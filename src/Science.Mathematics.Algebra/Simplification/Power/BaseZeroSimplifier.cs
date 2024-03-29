﻿using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions like 0 ^ x to 0.
/// </summary>
internal sealed class BaseZeroSimplifier : ISimplifier<PowerExpression>
{
	public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
	{
		if (expression.Base.GetConstantValue(cancellationToken) == 0)
		{
			if (expression.Exponent.GetConstantValue(cancellationToken) == 0)
				return NumberExpression.One;

			return NumberExpression.Zero;
		}

		return expression;
	}
}
