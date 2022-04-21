using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents an absolute value expression.
/// </summary>
public class InfinityExpression : AlgebraExpression, IEquatable<InfinityExpression>
{
	public InfinityExpression()
	{
	}


	public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken)) => null;


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this;

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield break;
	}


	#region Immutability
	public AbsoluteValueExpression WithExpression(AlgebraExpression newExpression) => ExpressionFactory.AbsoluteValue(newExpression);
	#endregion


	public override string ToString() => $"∞";

	public bool Equals(InfinityExpression other) => other != null;

	public override int GetHashCode() => Int32.MaxValue;

	public override bool Equals(object obj) => obj is InfinityExpression;
}

public static partial class ExpressionFactory
{
	public static InfinityExpression Infinity() => new InfinityExpression();
}
