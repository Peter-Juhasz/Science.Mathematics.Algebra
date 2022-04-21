using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents an absolute value expression.
/// </summary>
public record class InfinityExpression : AlgebraExpression, IEquatable<InfinityExpression>
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken)) => null;


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this;

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield break;
	}

	public override string ToString() => $"∞";

	public override int GetHashCode() => Int32.MaxValue;
}

public static partial class ExpressionFactory
{
	public static InfinityExpression Infinity() => new();
}
