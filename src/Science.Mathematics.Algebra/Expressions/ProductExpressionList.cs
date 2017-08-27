﻿using System;
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

        public ProductExpressionList WithTerms(IImmutableList<AlgebraExpression> newTerms)
        {
            return ExpressionFactory.Product(newTerms);
        }
        #endregion


        public bool Equals(ProductExpressionList other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (this.Terms.Count != other.Terms.Count)
                return false;

            return this.Terms.Zip(other.Terms, (x, y) => x.Equals(y)).All(b => b);
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as ProductExpressionList);
        }

        public override int GetHashCode()
        {
            return this.Terms.Select(o => o.GetHashCode()).Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            return String.Join(" * ", this.Terms
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
        public static ProductExpressionList Negate(AlgebraExpression expression)
        {
            return Multiply(NumberExpression.MinusOne, expression);
        }

        public static ProductExpressionList Multiply(AlgebraExpression left, AlgebraExpression right)
        {
            return new ProductExpressionList(ImmutableList.Create(left, right));
        }
        public static ProductExpressionList Divide(AlgebraExpression left, AlgebraExpression right)
        {
            return Multiply(left, Exponentiate(right, NumberExpression.MinusOne));
        }

        public static PowerExpression Reciprocal(AlgebraExpression expression)
        {
            return Exponentiate(expression, NumberExpression.MinusOne);
        }

        public static ProductExpressionList Product(IReadOnlyCollection<AlgebraExpression> terms)
        {
            return new ProductExpressionList(terms.ToImmutableList());
        }
        public static ProductExpressionList Product(params AlgebraExpression[] terms)
        {
            return Product(terms as IReadOnlyCollection<AlgebraExpression>);
        }
    }
}
