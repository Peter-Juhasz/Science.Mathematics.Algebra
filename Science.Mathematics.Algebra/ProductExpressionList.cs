using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Science.Mathematics.Algebra
{
    public class ProductExpressionList : ExpressionList, IEquatable<ProductExpressionList>
    {
        public ProductExpressionList(IEnumerable<AlgebraExpression> terms)
            : base(terms)
        { }
        

        public override AlgebraExpression Simplify()
        {
            // Simplify terms
            var simplifiedTerms = this.Terms.Select(t => t.Simplify());


            // Collect constants
            var constants = simplifiedTerms.ToDictionary(e => e, e => e.GetConstantValue());


            // all constant: 1 * 2 * 3  =>  6
            if (constants.Values.All(v => v != null))
                return ExpressionFactory.Constant(constants.Values.Cast<double>().Product(v => v));

            // collect constants: 1 * ? * 3  =>  (1 * 3) * ?  =>  3 * ?
            double product = constants.Values.Where(v => v != null).Cast<double>().Product(v => v);
            var newTerms = this.Terms.RemoveAll(e => constants[e].HasValue);

            if (product == 0) // 0 * ?  =>  0
                return ConstantExpression.Zero;

            if (product != 1) // 1 * ?  =>  ?
                newTerms = newTerms.Insert(0, ExpressionFactory.Constant(product));


            // TODO: Collect power terms

            return ExpressionFactory.Product(newTerms);
        }

        public override AlgebraExpression Expand()
        {
            throw new NotImplementedException();
        }

        public override AlgebraExpression Factor()
        {
            throw new NotImplementedException();
        }


        public override double? GetConstantValue()
        {
            return this.Terms.Select(t => t.GetConstantValue()).Product(v => v);
        }


        public override AlgebraExpression Limit(AlgebraExpression expression, AlgebraExpression subject, LimitDirection direction = LimitDirection.Both)
        {
            throw new NotImplementedException();
        }

        public override AlgebraExpression Differentiate(AlgebraExpression respectTo)
        {
            return new SumExpressionList(
                this.Terms.Select(
                    exp => new ProductExpressionList(this.Terms.Where(e => !exp.Equals(e)).Union(new AlgebraExpression[] { exp.Differentiate(respectTo) }))
                )
            );
        }

        public override AlgebraExpression Integrate(AlgebraExpression respectTo)
        {
            throw new NotImplementedException();
        }


        public override AlgebraExpression Substitute(AlgebraExpression subject, AlgebraExpression replacement)
        {
            if (this == subject)
                return replacement;

            // TODO: If product expression, try replace childs

            return ExpressionFactory.Product(
                this.Terms
                    .Select(t => t == subject ? replacement : subject)
                    .Select(t => t.Substitute(subject, replacement))
            );
        }


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
