using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents a list of expression terms.
/// </summary>
public abstract record class ExpressionList(IImmutableList<AlgebraExpression> Terms) : AlgebraExpression, IReadOnlyList<AlgebraExpression>
{
	public AlgebraExpression this[int index] => ((IReadOnlyList<AlgebraExpression>)Terms)[index];

	public override IEnumerable<AlgebraExpression> Children() => Terms;

	public int Count => Terms.Count;

	public IEnumerator<AlgebraExpression> GetEnumerator() => Terms.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Terms).GetEnumerator();
}
