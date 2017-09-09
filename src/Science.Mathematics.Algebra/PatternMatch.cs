using System;
using System.Collections.Immutable;

namespace Science.Mathematics.Algebra
{
    public class PatternMatch
    {
        public PatternMatch(AlgebraExpression pattern, IImmutableDictionary<string, AlgebraExpression> values)
        {
            Pattern = pattern;
            Values = values;
        }
        public PatternMatch(AlgebraExpression pattern)
            : this(pattern, ImmutableDictionary<string, AlgebraExpression>.Empty)
        { }
        public PatternMatch(AlgebraExpression pattern, SymbolExpression symbol, AlgebraExpression value)
            : this(pattern, ImmutableDictionary<string, AlgebraExpression>.Empty.Add(symbol.Name, value))
        { }

        public AlgebraExpression Pattern { get; }

        public IImmutableDictionary<string, AlgebraExpression> Values { get; }

        public AlgebraExpression this[string symbol] => Values[symbol];
        public AlgebraExpression this[SymbolExpression symbol] => Values[symbol.Name];

        public PatternMatch AddValues(IImmutableDictionary<string, AlgebraExpression> values)
            => new PatternMatch(this.Pattern, this.Values.AddRange(values));

        public PatternMatch AddValues(PatternMatch otherMatch) => AddValues(otherMatch.Values);
    }
}