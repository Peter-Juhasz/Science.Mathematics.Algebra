using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Provides simplification logic for a given type of <see cref="AlgebraExpression"/>.
/// </summary>
/// <typeparam name="TExpression"></typeparam>
public interface ISimplifier<in TExpression> where TExpression : AlgebraExpression
{
	/// <summary>
	/// Simplifies an <see cref="AlgebraExpression"/> of the desired kind.
	/// </summary>
	/// <param name="expression"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>The simplified expression.</returns>
	AlgebraExpression Simplify(TExpression expression, CancellationToken cancellationToken);
}
