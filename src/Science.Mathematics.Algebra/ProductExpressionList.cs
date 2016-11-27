using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    public class ProductExpressionList : ExpressionList, IEquatable<ProductExpressionList>
    {
        public ProductExpressionList(IReadOnlyCollection<AlgebraExpression> terms)
            : base(terms)
        { }
        

        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Terms.Select(t => t.GetConstantValue()).Product(v => v);
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
        public ProductExpressionList Multiply(AlgebraExpression expression)
        {
            return expression is ProductExpressionList
                ? ExpressionFactory.Product(this.Terms.Concat((expression as ProductExpressionList).Terms).ToImmutableList())
                : ExpressionFactory.Multiply(this, expression)
            ;
        }

        public ProductExpressionList Divide(AlgebraExpression expression)
        {
            return expression is ProductExpressionList
                ? ExpressionFactory.Product(this, ExpressionFactory.Reciprocal(expression))
                : ExpressionFactory.Divide(this, expression)
            ;
        }

        public ProductExpressionList WithTerms(IImmutableList<AlgebraExpression> terms)
        {
            return new ProductExpressionList(terms);
        }
        #endregion


        public bool Equals(ProductExpressionList other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (this.Terms.Count != other.Terms.Count)
                return false;

            return this.Terms.All(t => other.Terms.Contains(t)); // TODO: Add equality comparer
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj as ProductExpressionList);
        }

        public override int GetHashCode()
        {
            return this.Terms.Select(o => o.GetHashCode()).Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            return String.Join(" * ", this.Terms);
        }
    }
}
