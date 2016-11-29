using System;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions.
    /// </summary>
    internal sealed class ConstantPowerSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            double? @base = expression.Base.GetConstantValue(cancellationToken),
                exponent = expression.Exponent.GetConstantValue(cancellationToken);

            if (@base != null && exponent != null)
                return ExpressionFactory.Constant(Math.Pow(@base.Value, exponent.Value));

            return exponent;
        }
    }
}
