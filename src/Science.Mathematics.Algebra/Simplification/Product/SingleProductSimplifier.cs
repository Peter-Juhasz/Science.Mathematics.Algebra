using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions to their constant value, if possible.
/// </summary>
internal sealed class SingleProductSimplifier : ISimplifier<ProductExpressionList>
{
	public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken) => expression switch
	{
		[AlgebraExpression single] => single,
		_ => expression
	};
}
