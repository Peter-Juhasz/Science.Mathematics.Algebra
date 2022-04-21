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
            AddRule(0 ^ 0, 0);
            AddRule(x => x ^ 0, 1);
            AddRule(x => x ^ 1, x => x);
            AddRule(x => 1 ^ x, x => 1);
            AddRule(x => 0 ^ x, 0);
            AddRule(() => i ^ 2, () => 1);
            AddRule((a, b, c) => (a ^ b) ^ c, (a, b, c) => a ^ (b * c));
            AddRule(
                (a, x) => IndefiniteIntegral(a ^ x, x),
                (a, x) => (a ^ x) / NaturalLogarithm(a) + IntegralConstant,
                (a, x) => !a.DependsUpon(x)
            );
            AddRule(
               (a, x) => IndefiniteIntegral(x ^ a, x),
               (a, x) => (x ^ (a + 1)) / (a + 1) + IntegralConstant,
               (a, x) => a.GetConstantValue() != -1
            );
            AddRule(
                (a, x) => IndefiniteIntegral(e ^ (a * x), x),
                (a, x) => (e ^ (a * x)) / a + IntegralConstant,
                (a, x) => !a.DependsUpon(x)
            );

            // differentiation
            AddRule(x => Differentiate(e ^ x, x), x => e ^ x);
            AddRule((f, g, x) => Differentiate(f ^ g, x), (f, g, x) => (f ^ (g - 1)) * (g * f.Differentiation(x) + f * NaturalLogarithm(f) * g.Differentiation(x)));
            AddRule(x => Sine(x).Differentiation(x), x => Cosine(x));

            // sums
            AddRule(x => x + 0, x => x);
            AddRule(x => x - 0, x => x);
            AddRule((c, x, y) => (c * x) + (c * y), (c, x, y) => c * (x + y));

            // products
            AddRule(x => x * 1, x => x);
            AddRule(x => x / 1, x => x);
            AddRule((b, x, y) => (b ^ x) * (b ^ y), (b, x, y) => b ^ (x + y));
            AddRule(
                x => IndefiniteIntegral(1 / x, x),
                x => NaturalLogarithm(AbsoluteValue(x)) + IntegralConstant
            );

            // absolute value
            AddRule(() => AbsoluteValue(0), () => 0);
            AddRule(x => AbsoluteValue(AbsoluteValue(x)), x => AbsoluteValue(x));

            // trigonometry
            AddRule(
                (x, n) => IndefiniteIntegral(Sine(x) ^ n, x),
                (x, n) => -Exponentiation(Sine(x), n - 1) * Cosine(x) / n + (n - 1) / n * IndefiniteIntegral(Sine(x) ^ (n - 2), x) + IntegralConstant
            );
            AddRule(
                (x, n) => IndefiniteIntegral(Cosine(x) ^ n, x),
                (x, n) => Exponentiation(Cosine(x), n - 1) * Sine(x) / n + (n - 1) / n * IndefiniteIntegral(Cosine(x) ^ (n - 2), x) + IntegralConstant
            );

            // logarithms
            AddRule(
                x => IndefiniteIntegral(NaturalLogarithm(x), x),
                x => x * NaturalLogarithm(x) - x + IntegralConstant
            );
            AddRule(
                (x, a) => IndefiniteIntegral(Logarithm(x, a), x),
                (x, a) => x * Logarithm(x, a) - x / NaturalLogarithm(a) + IntegralConstant
            );

            // integrals
            AddRule(
                () => IntegralConstant + IntegralConstant,
                () => IntegralConstant
            );
            AddRule(
                (x, n, c) => IndefiniteIntegral((x ^ n) * Exponentiation(e, c * x), x),
                (x, n, c) => (x ^ n) * Exponentiation(e, c * x) / c - (n / c) * IndefiniteIntegral(Exponentiation(x, n -1) * Exponentiation(e, c * x), x)
            );

            // euler's identity
            AddRule(e ^ (Pi * i), -1);
        }

        public static ICollection<ExpressionTransformationRule> Rules { get; } = new HashSet<ExpressionTransformationRule>();

        public static void AddRule(
            AlgebraExpression pattern,
            AlgebraExpression result,
            params Func<SymbolExpression, bool>[] conditions
        )
        {
            Rules.Add(new ExpressionTransformationRule(pattern, result));
        }

        public static void AddRule(
            Expression<Func<AlgebraExpression>> pattern,
            Expression<Func<AlgebraExpression>> result,
            params Func<SymbolExpression, bool>[] conditions
        )
        {
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(), result.Compile()()));
        }

        public static void AddRule(
            Expression<Func<AlgebraExpression>> pattern,
            AlgebraExpression result,
            params Func<SymbolExpression, bool>[] conditions
        )
        {
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(), result));
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
            Expression<Func<SymbolExpression, AlgebraExpression>> pattern,
            AlgebraExpression result,
            params Func<SymbolExpression, bool>[] conditions
        )
        {
            var p1 = Symbol(pattern.Parameters[0].Name);
            Rules.Add(new ExpressionTransformationRule(pattern.Compile()(p1), result));
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
            MatchContext context = new MatchContext();

            do
            {
                lastResult = simplified;

                foreach (var pattern in Rules)
                {
                    var match = pattern.Pattern.MatchTo(lastResult, context, cancellationToken).FirstOrDefault();
                    if (match != null)
                    {
                        simplified = pattern.Result.Substitute(match.Values);
                        continue;
                    }
                }
            } while (!lastResult.Equals(simplified));

            return simplified;
        }
    }
}
