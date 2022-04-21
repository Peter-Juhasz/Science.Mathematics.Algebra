using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions.
/// </summary>
internal sealed class GenericPowerSimplifier : ISimplifier<PowerExpression>
{
	public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken) => expression
			.WithBase(expression.Base.Simplify())
			.WithExponent(expression.Exponent.Simplify())
		;
}
