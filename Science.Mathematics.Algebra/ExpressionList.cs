using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Science.Mathematics.Algebra
{
    public abstract class ExpressionList : AlgebraExpression
    {
        public ExpressionList(IEnumerable<AlgebraExpression> terms)
            : this(terms.ToImmutableList())
        { }
        public ExpressionList(ImmutableList<AlgebraExpression> terms)
        {
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));

            if (terms.Count == 0)
                throw new ArgumentException("Expression list must contain at least one term.", nameof(terms));

            this.Terms = terms;
        }

        public ImmutableList<AlgebraExpression> Terms { get; private set; }
    }
}
