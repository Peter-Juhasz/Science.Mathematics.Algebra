using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents the sum of multiple terms.
    /// </summary>
    public class SumExpressionList : ExpressionList, IEquatable<SumExpressionList>
    {
        public SumExpressionList(IReadOnlyCollection<AlgebraExpression> terms)
            : base(terms)
        { }
        
        
        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            var values = this.Terms.Select(t => t.GetConstantValue()).Memoize();

            if (values.Contains(null))
                return null;

            return values.Sum();
        }

        
        public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement)
        {
            return this.WithTerms(
                this.Terms
                    .Select(t => t == variable ? replacement : variable)
                    .Select(t => t.Substitute(variable, replacement))
                    .ToImmutableList()
            );
        }


        #region Immutability
        public SumExpressionList Add(AlgebraExpression expression)
        {
            return expression is SumExpressionList
                ? ExpressionFactory.Sum(this.Terms.Concat((expression as SumExpressionList).Terms).ToImmutableList())
                : ExpressionFactory.Add(this, expression)
            ;
        }

        public SumExpressionList Subtract(AlgebraExpression expression)
        {
            return expression is SumExpressionList
                ? ExpressionFactory.Sum(this.Terms.Concat((expression as SumExpressionList).Terms.Select(ExpressionFactory.Negate)).ToImmutableList())
                : ExpressionFactory.Subtract(this, expression)
            ;
        }

        public SumExpressionList WithTerms(IImmutableList<AlgebraExpression> newTerms)
        {
            return ExpressionFactory.Sum(newTerms);
        }
        #endregion


        internal AlgebraExpression Normalize()
        {
            if (this.Terms.Count == 1)
                return this.Terms.Single();

            return this;
        }


        public bool Equals(SumExpressionList other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (this.Terms.Count != other.Terms.Count)
                return false;

            return this.Terms.Zip(other.Terms, (x, y) => x.Equals(y)).All(b => b);
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as SumExpressionList);
        }

        public override int GetHashCode()
        {
            return this.Terms.Select(o => o.GetHashCode()).Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            return String.Join(" + ", this.Terms
                .Select(t => NeedsParenthesis(t) ? $"({t})" : t.ToString())
            );
        }

        private static bool NeedsParenthesis(AlgebraExpression expression)
        {
            return expression is SumExpressionList;
        }
    }

    public static partial class ExpressionFactory
    {
        public static SumExpressionList Add(AlgebraExpression left, AlgebraExpression right)
        {
            return new SumExpressionList(ImmutableList.Create(left, right));
        }
        public static SumExpressionList Subtract(AlgebraExpression left, AlgebraExpression right)
        {
            return Add(left, Negate(right));
        }

        public static SumExpressionList Sum(IReadOnlyCollection<AlgebraExpression> terms)
        {
            return new SumExpressionList(terms.ToImmutableList());
        }
        public static SumExpressionList Sum(params AlgebraExpression[] terms)
        {
            return Sum(terms as IReadOnlyCollection<AlgebraExpression>);
        }
    }
}
