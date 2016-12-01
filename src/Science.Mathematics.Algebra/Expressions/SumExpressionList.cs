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

            if (values.All(v => v == null))
                return null;

            return values.Sum();
        }

        
        public override AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement)
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

        public SumExpressionList WithTerms(IImmutableList<AlgebraExpression> terms)
        {
            return ExpressionFactory.Sum(terms);
        }
        #endregion


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
            return String.Join(" + ", this.Terms);
        }
    }
}
