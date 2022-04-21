using System.Collections.Immutable;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents the sine function.
/// </summary>
[FunctionName(PrimaryName)]
public class SineFunctionExpression : FunctionInvocationExpression
{
	public SineFunctionExpression(AlgebraExpression argument)
		: base(PrimaryName, ImmutableList<AlgebraExpression>.Empty.Add(argument))
	{ }

	public const string PrimaryName = "sin";

	public AlgebraExpression Argument => this.Arguments[0];
}

public static partial class WellKnownFunctionNames
{
	public const string Sine = SineFunctionExpression.PrimaryName;
}

public static partial class ExpressionFactory
{
	/// <summary>
	/// Creates a sine function invocation expression.
	/// </summary>
	public static SineFunctionExpression Sine(AlgebraExpression argument) => Invoke(WellKnownFunctionNames.Sine, argument) as SineFunctionExpression;
}
