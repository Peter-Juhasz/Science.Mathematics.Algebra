using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions.
/// </summary>
internal sealed class GenericPowerSimplifier : ISimplifier<PowerExpression>
{
	public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken) => expression with
	{
		Base = expression.Base.Simplify(cancellationToken),
		Exponent = expression.Exponent.Simplify(cancellationToken)
	};
}
