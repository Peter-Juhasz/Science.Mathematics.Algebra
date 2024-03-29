﻿using System.Collections.Immutable;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents the cosine function.
/// </summary>
[FunctionName(PrimaryName)]
public record class CosineFunctionExpression : FunctionInvocationExpression
{
	public CosineFunctionExpression(AlgebraExpression argument)
		: base(PrimaryName, ImmutableList<AlgebraExpression>.Empty.Add(argument))
	{ }

	public const string PrimaryName = "cos";

	public AlgebraExpression Argument => Arguments[0];
}

public static partial class WellKnownFunctionNames
{
	public const string Cosine = CosineFunctionExpression.PrimaryName;
}

public static partial class ExpressionFactory
{
	/// <summary>
	/// Creates a sine function invocation expression.
	/// </summary>
	public static CosineFunctionExpression Cosine(AlgebraExpression argument) => Invoke(WellKnownFunctionNames.Cosine, argument) as CosineFunctionExpression;
}
