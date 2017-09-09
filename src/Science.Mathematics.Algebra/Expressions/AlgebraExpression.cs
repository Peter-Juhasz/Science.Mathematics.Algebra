using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    using System.Linq.Expressions;
    using static ExpressionFactory;

    /// <summary>
    /// Represents an algebra expression. Serves as the base class of all expressions.
    /// </summary>
    public abstract class AlgebraExpression
    {
        /// <summary>
        /// Computes the constant value of the expression.
        /// </summary>
        /// <returns></returns>
        public abstract double? GetConstantValue(CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces every occurrences of <paramref name="subject"/> to <paramref name="replacement"/>.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public abstract AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement);

        public abstract IEnumerable<AlgebraExpression> Children();

        public virtual bool IsInfinity() => this.DescendantsAndSelf().OfType<InfinityExpression>().Any();

        public virtual IEnumerable<PatternMatch> MatchTo(AlgebraExpression expression, CancellationToken cancellationToken = default)
        {
            yield break;
        }


        #region Operators
        public static SumExpressionList operator +(AlgebraExpression left, AlgebraExpression right)
        {
            return Add(left, right);
        }
        public static SumExpressionList operator +(double left, AlgebraExpression right)
        {
            return Add(Number(left), right);
        }
        public static SumExpressionList operator +(AlgebraExpression left, double right)
        {
            return Add(left, Number(right));
        }

        public static SumExpressionList operator -(AlgebraExpression left, AlgebraExpression right)
        {
            return Subtract(left, right);
        }
        public static SumExpressionList operator -(double left, AlgebraExpression right)
        {
            return Subtract(Number(left), right);
        }
        public static SumExpressionList operator -(AlgebraExpression left, double right)
        {
            return Subtract(left, Number(right));
        }

        public static ProductExpressionList operator *(AlgebraExpression left, AlgebraExpression right)
        {
            return Multiply(left, right);
        }
        public static ProductExpressionList operator *(double left, AlgebraExpression right)
        {
            return Multiply(Number(left), right);
        }
        public static ProductExpressionList operator *(AlgebraExpression left, double right)
        {
            return Multiply(left, Number(right));
        }

        public static ProductExpressionList operator /(AlgebraExpression left, AlgebraExpression right)
        {
            return Divide(left, right);
        }
        public static ProductExpressionList operator /(double left, AlgebraExpression right)
        {
            return Divide(Number(left), right);
        }
        public static ProductExpressionList operator /(AlgebraExpression left, double right)
        {
            return Divide(left, Number(right));
        }

        public static PowerExpression operator ^(AlgebraExpression left, AlgebraExpression right)
        {
            return Exponentiation(left, right);
        }
        public static PowerExpression operator ^(double left, AlgebraExpression right)
        {
            return Exponentiation(Number(left), right);
        }
        public static PowerExpression operator ^(AlgebraExpression left, double right)
        {
            return Exponentiation(left, Number(right));
        }

        public static ProductExpressionList operator -(AlgebraExpression expr)
        {
            return Negate(expr);
        }

        public static bool operator ==(AlgebraExpression left, AlgebraExpression right)
        {
            if (Object.ReferenceEquals(left, right)) return true;

            return left.Equals(right);
        }
        public static bool operator !=(AlgebraExpression left, AlgebraExpression right)
        {
            return !(left == right);
        }
        #endregion

        public override bool Equals(object obj)
        {
            return this == obj as AlgebraExpression;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsEquivalentTo(AlgebraExpression other)
        {
            if (this.Equals(other))
                return true;

            return this.Simplify().Equals(other.Simplify());
        }

        #region Conversions
        public static implicit operator AlgebraExpression(int value)
        {
            return Constant(value);
        }

        public static implicit operator AlgebraExpression(double value)
        {
            return Number(value);
        }

        public static implicit operator AlgebraExpression(string name)
        {
            return ExpressionFactory.Symbol(name);
        }

        public static implicit operator AlgebraExpression(char name)
        {
            return ExpressionFactory.Symbol(name);
        }
        #endregion
    }

    public static partial class AlgebraExpressionExtensions
    {
        public static bool IsConstant(this AlgebraExpression expression, SymbolExpression respectTo)
        {
            return !expression.DependsUpon(respectTo);
        }

        public static bool DependsUpon(this AlgebraExpression expression, SymbolExpression respectTo)
        {
            return expression.DescendantsAndSelf().Contains(respectTo);
        }


        public static IEnumerable<AlgebraExpression> Descendants(this AlgebraExpression expression)
        {
            foreach (var child in expression.Children())
            {
                foreach (var descendant in child.DescendantsAndSelf())
                    yield return descendant;
            }
        }

        public static IEnumerable<AlgebraExpression> DescendantsAndSelf(this AlgebraExpression expression)
        {
            yield return expression;

            foreach (var descendant in expression.Descendants())
                yield return descendant;
        }

        public static IEnumerable<PatternMatch> Match(this AlgebraExpression subject, AlgebraExpression pattern, CancellationToken cancellationToken = default)
        {
            return pattern.MatchTo(subject, cancellationToken);
        }
        public static IEnumerable<PatternMatch> Match(
            this AlgebraExpression subject,
            Expression<Func<SymbolExpression, AlgebraExpression>> pattern,
            CancellationToken cancellationToken = default)
        {
            var p1 = Symbol(pattern.Parameters[0].Name);
            return subject.MatchTo(pattern.Compile()(p1), cancellationToken);
        }


        public static AlgebraExpression Substitute(this AlgebraExpression expression, IReadOnlyDictionary<SymbolExpression, AlgebraExpression> map)
        {
            return map.Aggregate(expression, (r, kv) => r.Substitute(kv.Key, kv.Value));
        }
        public static AlgebraExpression Substitute(this AlgebraExpression expression, IReadOnlyDictionary<string, AlgebraExpression> map)
        {
            return map.Aggregate(expression, (r, kv) => r.Substitute(kv.Key, kv.Value));
        }
    }
}
