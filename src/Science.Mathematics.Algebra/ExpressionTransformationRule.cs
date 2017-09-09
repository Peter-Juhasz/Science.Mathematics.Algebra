namespace Science.Mathematics.Algebra
{
    public class ExpressionTransformationRule
    {
        public ExpressionTransformationRule(AlgebraExpression pattern, AlgebraExpression result)
        {
            Pattern = pattern;
            Result = result;
        }

        public AlgebraExpression Pattern { get; }

        public AlgebraExpression Result { get; }
    }
}
