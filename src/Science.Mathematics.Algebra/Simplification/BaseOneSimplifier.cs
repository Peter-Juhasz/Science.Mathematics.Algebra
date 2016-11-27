﻿using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like 1 ^ x to 1.
    /// </summary>
    public sealed class BaseOneSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Base.GetConstantValue(cancellationToken) == 1)
                return ConstantExpression.One;

            return expression;
        }
    }
}