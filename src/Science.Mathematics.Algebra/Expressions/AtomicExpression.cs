using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents an atomic expression.
/// </summary>
public abstract record class AtomicExpression : AlgebraExpression
{
	public override IEnumerable<AlgebraExpression> Children() => Enumerable.Empty<AlgebraExpression>();
}
