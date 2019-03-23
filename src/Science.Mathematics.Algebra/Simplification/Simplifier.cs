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
            // powers
            Simplifier.AddRule(x => x ^ 0, x => 1);
            Simplifier.AddRule(x => x ^ 1, x => x);
            Simplifier.AddRule(x => 1 ^ x, x => 1);
            Simplifier.AddRule(x => 0 ^ x, x => 0);
            Simplifier.AddRule((a, b, c) => (a ^ b) ^ c, (a, b, c) => a ^ (b * c));
            Simplifier.AddRule(
                (a, x) => IndefiniteIntegral(a ^ x, x),
                (a, x) => (a ^ x) / NaturalLogarithm(a) + IntegralConstant,
                (a, x) => !a.DependsUpon(x)
            );
            Simplifier.AddRule(
               (a, x) => IndefiniteIntegral(x ^ a, x),
               (a, x) => (x ^ (a + 1)) / (a + 1) + IntegralConstant,
               (a, x) => a.GetConstantValue() != -1
            );
            Simplifier.AddRule(
                (a, x) => IndefiniteIntegral(e ^ (a * x), x),
                (a, x) => (e ^ (a * x)) / a + IntegralConstant,
                (a, x) => !a.DependsUpon(x)
            );

            // sums
            Simplifier.AddRule(x => x + 0, x => x);
            Simplifier.AddRule(x => x - 0, x => x);
            Simplifier.AddRule((c, x, y) => (c * x) + (c * y), (c, x, y) => c * (x + y));

            // products
            Simplifier.AddRule(x => x * 1, x => x);
            Simplifier.AddRule(x => x / 1, x => x);
            Simplifier.AddRule((b, x, y) => (b ^ x) * (b ^ y), (b, x, y) => b ^ (x + y));
            Simplifier.AddRule(
                x => IndefiniteIntegral(1 / x, x),
                x => NaturalLogarithm(AbsoluteValue(x)) + IntegralConstant
            );

            // absolute value
            Simplifier.AddRule(x => AbsoluteValue(AbsoluteValue(x)), x => AbsoluteValue(x));

            // trigonometry
            Simplifier.AddRule(
                (x, n) => IndefiniteIntegral(Sine(x) ^ n, x),
                (x, n) => -Exponentiation(Sine(x), n - 1) * Cosine(x) / n + (n - 1) / n * IndefiniteIntegral(Sine(x) ^ (n - 2), x) + IntegralConstant
            );
            Simplifier.AddRule(
                (x, n) => IndefiniteIntegral(Cosine(x) ^ n, x),
                (x, n) => Exponentiation(Cosine(x), n - 1) * Sine(x) / n + (n - 1) / n * IndefiniteIntegral(Cosine(x) ^ (n - 2), x) + IntegralConstant
            );

            // logarithms
            Simplifier.AddRule(
                x => IndefiniteIntegral(NaturalLogarithm(x), x),
                x => x * NaturalLogarithm(x) - x + IntegralConstant
            );
            Simplifier.AddRule(
                (x, a) => IndefiniteIntegral(Logarithm(x, a), x),
                (x, a) => x * Logarithm(x, a) - x / NaturalLogarithm(a) + IntegralConstant
            );

            // integrals
            Simplifier.AddRule(
                () => IntegralConstant + IntegralConstant,
                () => IntegralConstant
            );
            Simplifier.AddRule(
                (x, n, c) => IndefiniteIntegral((x ^ n) * Exponentiation(e, c * x), x),
                (x, n, c) => (x ^ n) * Exponentiation(e, c * x) / c - (n / c) * IndefiniteIntegral(Exponentiation(x, n -1) * Exponentiation(e, c * x), x)
            );
        }

        public static ICollection<ExpressionTransformationRule> Rules { get; } = new HashSet<ExpressionTransformationRule>();

        public static void AddRule(
            Expression<Func<AlgebraExpression>> pattern,
            Expression<Func<AlgebraExpression>> result,
            params Func<SymbolExpression, bool>[] conditions
        )
        {
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(), result.Compile()()));
        }

        public static void AddRule(
            Expression<Func<SymbolExpression, AlgebraExpression>> pattern,
            Expression<Func<SymbolExpression, AlgebraExpression>> result,
            params Func<SymbolExpression, bool>[] conditions
        )
        {
            var p1 = Symbol(pattern.Parameters[0].Name);
            var r1 = Symbol(result.Parameters[0].Name);
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(p1), result.Compile()(r1)));
        }

        public static void AddRule(
            Expression<Func<SymbolExpression, SymbolExpression, AlgebraExpression>> pattern,
            Expression<Func<SymbolExpression, SymbolExpression, AlgebraExpression>> result,
            params Func<SymbolExpression, SymbolExpression, bool>[] conditions
        )
        {
            var p1 = Symbol(pattern.Parameters[0].Name);
            var p2 = Symbol(pattern.Parameters[1].Name);
            var r1 = Symbol(result.Parameters[0].Name);
            var r2 = Symbol(result.Parameters[1].Name);
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(p1, p2), result.Compile()(r1, r2)));
        }

        public static void AddRule(
            Expression<Func<SymbolExpression, SymbolExpression, SymbolExpression, AlgebraExpression>> pattern,
            Expression<Func<SymbolExpression, SymbolExpression, SymbolExpression, AlgebraExpression>> result,
            params Func<SymbolExpression, SymbolExpression, SymbolExpression, bool>[] conditions
        )
        {
            var p1 = Symbol(pattern.Parameters[0].Name);
            var p2 = Symbol(pattern.Parameters[1].Name);
            var p3 = Symbol(pattern.Parameters[2].Name);
            var r1 = Symbol(result.Parameters[0].Name);
            var r2 = Symbol(result.Parameters[1].Name);
            var r3 = Symbol(result.Parameters[2].Name);
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(p1, p2, p3), result.Compile()(r1, r2, r3)));
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
