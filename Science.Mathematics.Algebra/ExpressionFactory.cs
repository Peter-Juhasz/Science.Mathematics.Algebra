namespace Science.Mathematics.Algebra
{
    public static class ExpressionFactory
    {
        public static ConstantExpression Constant(int value)
        {
            return new ConstantExpression(value);
        }
        
        public static VariableExpression Variable(string name)
        {
            return new VariableExpression(name);
        }
    }
}
