using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents the sum of multiple terms.
    /// </summary>
    public class SumExpressionList : ExpressionList, IEquatable<SumExpressionList>
    {
        public SumExpressionList(IEnumerable<AlgebraExpression> terms)
            : base(terms)
        { }
        

        public override AlgebraExpression Simplify()
        {
            // Simplify terms
            var simplifiedTerms = this.Terms.Select(t => t.Simplify());


            // Collect constants
            var constants = simplifiedTerms.ToDictionary(e => e, e => e.GetConstantValue());

            // all constant: 1 + 2 + 3  =>  6
            if (constants.Values.All(v => v != null))
                return ExpressionFactory.Constant(constants.Values.Cast<double>().Sum(v => v));

            // collect constants: 1 + ? + 3  =>  (1 + 3) + ?  =>  4 + ?
            double sum = constants.Values.Where(v => v != null).Cast<double>().Sum(v => v);
            var newTerms = this.Terms.RemoveAll(e => constants[e].HasValue);

            if (sum != 0) // 0 + ?  =>  ?
                newTerms = newTerms.Add(ExpressionFactory.Constant(sum));


            // TODO: Collect multiplication terms

            return ExpressionFactory.Sum(newTerms);
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
            return this.Terms.Sum(t => t.GetConstantValue());
        }


        public override AlgebraExpression Limit(AlgebraExpression expression, AlgebraExpression subject, LimitDirection direction = LimitDirection.Both)
        {
            return new SumExpressionList(
                this.Terms.Select(t => t.Limit(expression, subject, direction)).ToImmutableList()
            );
        }

        public override AlgebraExpression Differentiate(AlgebraExpression respectTo)
        {
            return new SumExpressionList(
                this.Terms.Select(t => t.Differentiate(respectTo)).ToImmutableList()
            );
        }

        public override AlgebraExpression Integrate(AlgebraExpression respectTo)
        {
            return new SumExpressionList(
                this.Terms.Select(t => t.Integrate(respectTo)).ToImmutableList()
            );
        }


        public override AlgebraExpression Substitute(AlgebraExpression subject, AlgebraExpression replacement)
        {
            if (this == subject)
                return replacement;

            // TODO: If sum expression, try replace childs

            return ExpressionFactory.Sum(
                this.Terms
                    .Select(t => t == subject ? replacement : subject)
                    .Select(t => t.Substitute(subject, replacement))
            );
        }


        #region Immatability
        public SumExpressionList Add(AlgebraExpression expression)
        {
            return expression is SumExpressionList
                ? ExpressionFactory.Sum(this.Terms.Concat((expression as SumExpressionList).Terms))
                : ExpressionFactory.Add(this, expression)
            ;
        }

        public SumExpressionList Subtract(AlgebraExpression expression)
        {
            return expression is SumExpressionList
                ? ExpressionFactory.Sum(this.Terms.Concat((expression as SumExpressionList).Terms.Select(ExpressionFactory.Negate)))
                : ExpressionFactory.Subtract(this, expression)
            ;
        }
        #endregion


        public bool Equals(SumExpressionList other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (this.Terms.Count != other.Terms.Count)
                return false;

            return this.Terms.All(t => other.Terms.Contains(t)); // TODO: Add equality comparer
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj as SumExpressionList);
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
