using System.Collections.Immutable;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents the cosine function.
    /// </summary>
    [FunctionName(PrimaryName)]
    public class CosineFunctionExpression : FunctionInvocationExpression
    {
        public CosineFunctionExpression(IImmutableList<AlgebraExpression> arguments)
            : base(PrimaryName, arguments)
        { }

        public const string PrimaryName = "cos";
    }

    public static partial class WellKnownFunctionNames
    {
        public const string Cosine = CosineFunctionExpression.PrimaryName;
    }

    public static partial class ExpressionFactory
    {
        /// <summary>
        /// Create a sine function invocation expression.
        /// </summary>
        public static CosineFunctionExpression Cosine(AlgebraExpression argument)
        {
            return Invoke(WellKnownFunctionNames.Cosine, argument) as CosineFunctionExpression;
        }
    }
}
