using System.Collections.Immutable;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Represents the sine function.
    /// </summary>
    [FunctionName(PrimaryName)]
    public class LogarithmFunctionExpression : FunctionInvocationExpression
    {
        public LogarithmFunctionExpression(AlgebraExpression argument, AlgebraExpression @base)
            : base(PrimaryName, ImmutableList<AlgebraExpression>.Empty.Add(argument).Add(@base))
        { }
        public LogarithmFunctionExpression(AlgebraExpression argument)
            : this(argument, E)
        { }

        public const string PrimaryName = "log";


        public AlgebraExpression Base => this.Arguments[1];

        public AlgebraExpression Argument => this.Arguments[0];


        #region Immutability
        public LogarithmFunctionExpression WithBase(AlgebraExpression newBase)
        {
            return ExpressionFactory.Logarithm(this.Argument, newBase);
        }

        public LogarithmFunctionExpression WithArgument(AlgebraExpression newArgument)
        {
            return ExpressionFactory.Logarithm(newArgument, this.Base);
        }
        #endregion
    }

    public static partial class WellKnownFunctionNames
    {
        public const string Logarithm = LogarithmFunctionExpression.PrimaryName;
    }

    public static partial class ExpressionFactory
    {
        /// <summary>
        /// Creates a log function invocation expression.
        /// </summary>
        public static LogarithmFunctionExpression NaturalLogarithm(AlgebraExpression argument)
        {
            return Logarithm(argument, E);
        }

        /// <summary>
        /// Creates a log function invocation expression.
        /// </summary>
        public static LogarithmFunctionExpression Logarithm(AlgebraExpression argument, AlgebraExpression @base)
        {
            return Invoke(WellKnownFunctionNames.Logarithm, argument, @base) as LogarithmFunctionExpression;
        }
    }
}
