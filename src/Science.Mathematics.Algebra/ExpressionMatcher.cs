using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    public static class ExpressionMatcher
    {
        public static IEnumerable<PatternMatch> MatchVariations(this IEnumerable<AlgebraExpression> expressions, IEnumerable<AlgebraExpression> patterns, MatchContext context, CancellationToken cancellationToken = default)
        {
            var expression = expressions.First();
            var pattern = patterns.First();

            foreach (var match in pattern.MatchTo(expression, context, cancellationToken))
            {
                var other = expressions.Skip(1);

                if (other.Any())
                {
                    foreach (var childMatch in MatchVariations(other.Select(e => e.Substitute(match.Values)), patterns.Skip(1), context.WithMatches(match.Values), cancellationToken))
                    {
                        yield return childMatch;
                    }
                }
                else
                {
                    yield return match.AddValues(context.MatchedVariables);
                }
            }
        }
    }
}
