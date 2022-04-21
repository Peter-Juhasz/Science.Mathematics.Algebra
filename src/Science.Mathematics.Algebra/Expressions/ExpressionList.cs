using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents a list of expression terms.
/// </summary>
public abstract record class ExpressionList(IImmutableList<AlgebraExpression> Terms) : AlgebraExpression
{
	public override IEnumerable<AlgebraExpression> Children() => Terms;
}
