using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies <see cref="AlgebraExpression"/>s using all the simplifiers found in the library.
    /// </summary>
    public static class Simplifier
    {
        static Simplifier()
        {
            Simplifier.AddRule(x => x ^ 0, x => 1);
            Simplifier.AddRule(x => x ^ 1, x => x);
            Simplifier.AddRule(x => 1 ^ x, x => 1);
            Simplifier.AddRule(x => 0 ^ x, x => 0);

            Simplifier.AddRule(x => x + 0, x => x);
            Simplifier.AddRule(x => x - 0, x => x);
            Simplifier.AddRule(x => x * 1, x => x);
            Simplifier.AddRule(x => x / 1, x => x);
        }

        public static ICollection<ExpressionTransformationRule> Rules { get; } = new HashSet<ExpressionTransformationRule>();

        public static void AddRule(
            Expression<Func<SymbolExpression, AlgebraExpression>> pattern,
            Expression<Func<SymbolExpression, AlgebraExpression>> result
        )
        {
            var p1 = Symbol(pattern.Parameters[0].Name);
            var r1 = Symbol(result.Parameters[0].Name);
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(p1), result.Compile()(r1)));
        }

        public static void AddRule(
            Expression<Func<SymbolExpression, SymbolExpression, AlgebraExpression>> pattern,
            Expression<Func<SymbolExpression, SymbolExpression, AlgebraExpression>> result
        )
        {
            var p1 = Symbol(pattern.Parameters[0].Name);
            var p2 = Symbol(pattern.Parameters[1].Name);
            var r1 = Symbol(result.Parameters[0].Name);
            var r2 = Symbol(result.Parameters[2].Name);
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(p1, p2), result.Compile()(r1, r2)));
        }

        /// <summary>
        /// Simplifies an <see cref="AlgebraExpression"/> as much as it can.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static AlgebraExpression Simplify(this AlgebraExpression expression, CancellationToken cancellationToken = default)
        {
            AlgebraExpression simplified = expression;
            AlgebraExpression lastResult;

            do
            {
                lastResult = simplified;

                // simplify by kind
                simplified = SimplifySpecificKind(simplified, cancellationToken);
            } while (!lastResult.Equals(simplified));

            return simplified;
        }


        private static AlgebraExpression SimplifySpecificKind(AlgebraExpression expression, CancellationToken cancellationToken)
        {
            AlgebraExpression simplified = expression;
            AlgebraExpression lastResult;

            // find applicable simplifiers
            IReadOnlyCollection<object> simplifiers = GetSimplifiers(expression);

            do
            {
                lastResult = simplified;

                // query each simplifier
                foreach (var simplifier in simplifiers)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var original = simplified;

                    MethodInfo method = simplifier.GetType().GetRuntimeMethod(nameof(ISimplifier<AlgebraExpression>.Simplify), new[] { expression.GetType(), typeof(CancellationToken) });
                    simplified = method.Invoke(simplifier, new object[] { simplified, cancellationToken }) as AlgebraExpression;
                    
                    // trace
                    if (!original.Equals(simplified))
                        Debug.WriteLine($"Simplified '{original}' to '{simplified}' using '{simplifier}'");

                    // stop if not the same kind of expression
                    if (lastResult.GetType() != simplified.GetType())
                        return simplified;
                }
            } while (!lastResult.Equals(simplified));

            return simplified;
        }

        private static IEnumerable<Type> FindSimplifiers(Type expressionType)
        {
            return typeof(ISimplifier<>).GetTypeInfo().Assembly.GetTypes()
                .Where(t =>
                    t.GetTypeInfo().ImplementedInterfaces
                        .Where(i => i.IsConstructedGenericType)
                        .Where(i => i.GetGenericTypeDefinition() == typeof(ISimplifier<>))
                        .Any(i => i.GenericTypeArguments[0].GetTypeInfo().IsAssignableFrom(expressionType.GetTypeInfo()))
                )
            ;
        }

        private static IReadOnlyCollection<object> GetSimplifiers(AlgebraExpression expression)
        {
            return FindSimplifiers(expression.GetType())
                .Select(Activator.CreateInstance)
                .ToList();
        }
    }
}
