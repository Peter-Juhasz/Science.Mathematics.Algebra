using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions to their constant value, if possible.
/// </summary>
internal sealed class SingleSumSimplifier : ISimplifier<SumExpressionList>
{
	public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken) => expression switch
	{
		[AlgebraExpression single] => single,
		_ => expression
	};
}
