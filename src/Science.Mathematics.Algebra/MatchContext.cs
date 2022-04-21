using System.Collections.Generic;
using System.Collections.Immutable;

namespace Science.Mathematics.Algebra
{
    public class MatchContext
    {
        public MatchContext(IImmutableDictionary<string, AlgebraExpression> matchedVariables)
        {
            MatchedVariables = matchedVariables;
        }
        public MatchContext()
            : this(ImmutableDictionary<string, AlgebraExpression>.Empty)
        {

        }

        public IImmutableDictionary<string, AlgebraExpression> MatchedVariables { get; set; }


        public MatchContext WithMatch(string variable, AlgebraExpression match)
        {
            return new MatchContext(MatchedVariables.Add(variable, match));
        }

        public MatchContext WithMatches(IEnumerable<KeyValuePair<string, AlgebraExpression>> matches)
        {
            return new MatchContext(MatchedVariables.AddRange(matches));
        }
    }
}
