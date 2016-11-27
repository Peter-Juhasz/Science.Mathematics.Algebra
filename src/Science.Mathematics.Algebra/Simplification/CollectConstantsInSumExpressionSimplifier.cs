using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Collects constants in a sum expression.
    /// </summary>
    public sealed class CollectConstantsInSumExpressionSimplifier : ISimplifier<SumExpressionList>
    {
        public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken)
        {
            // simplify terms
            var simplifiedTerms = expression.Terms.Select(t => t.Simplify());


            // collect constants
            var constants = simplifiedTerms.ToDictionary(e => e, e => e.GetConstantValue());

            // all constant: 1 + 2 + 3  =>  6
            if (constants.Values.All(v => v != null))
                return ExpressionFactory.Constant(constants.Values.Cast<double>().Sum(v => v));


            // collect constants: 1 + ? + 3  =>  (1 + 3) + ?  =>  4 + ?
            double sum = constants.Values.Where(v => v != null).Cast<double>().Sum(v => v);
            var newTerms = expression.Terms.RemoveAll(e => constants[e].HasValue);

            if (sum != 0) // 0 + ?  =>  ?
                newTerms = newTerms.Add(ExpressionFactory.Constant(sum));
            

            return ExpressionFactory.Sum(newTerms);
        }
    }
}
