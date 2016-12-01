using System.Collections.Immutable;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents the sine function.
    /// </summary>
    [FunctionName(PrimaryName)]
    public class SineFunctionExpression : FunctionInvocationExpression
    {
        public SineFunctionExpression(IImmutableList<AlgebraExpression> arguments)
            : base(PrimaryName, arguments)
        { }

        public const string PrimaryName = "sin";
    }

    public static partial class WellKnownFunctionNames
    {
        public const string Sine = SineFunctionExpression.PrimaryName;
    }

    public static partial class ExpressionFactory
    {
        /// <summary>
        /// Create a sine function invocation expression.
        /// </summary>
        public static SineFunctionExpression Sine(AlgebraExpression argument)
        {
            return Invoke(WellKnownFunctionNames.Sine, argument) as SineFunctionExpression;
        }
    }
}
