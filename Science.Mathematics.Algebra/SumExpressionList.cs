using System;
using System.Collections.Immutable;
using System.Linq;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents the sum of multiple terms.
    /// </summary>
    public class SumExpressionList : ExpressionList
    {
        public SumExpressionList(ImmutableList<AlgebraExpression> terms)
            : base(terms)
        { }
        

        public override AlgebraExpression Simplify()
        {
            // Collect constants
            var constants = this.Terms.ToDictionary(e => e, e => e.GetConstantValue());

            // all constant: 1 + 2 + 3
            if (constants.Values.All(v => v != null))
                return ExpressionFactory.Constant(constants.Values.Cast<double>().Sum(v => v));

            // collect constants: 1 + ? + 2 -> (1 + 2) + ? -> 3 + ?
            double sum = constants.Values.Where(v => v != null).Cast<double>().Sum(v => v);
            var newTerms = this.Terms.RemoveAll(e => constants[e].HasValue);

            if (sum != 0) // 0 + ?
                newTerms = newTerms.Add(ExpressionFactory.Constant(sum));

            // TODO: Collect multiplication terms

            return new SumExpressionList(newTerms);
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


        public override string ToString()
        {
            return String.Join(" + ", this.Terms);
        }
    }
}
