namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents an algebra expression. Serves as the base class of all expressions.
    /// </summary>
    public abstract class AlgebraExpression
    {
        #region Simple algebraic transformations
        /// <summary>
        /// Simplifies the expression.
        /// </summary>
        /// <returns></returns>
        public abstract AlgebraExpression Simplify();

        /// <summary>
        /// Expands the expression.
        /// </summary>
        /// <returns></returns>
        public abstract AlgebraExpression Expand();

        /// <summary>
        /// Factors the expression.
        /// </summary>
        /// <returns></returns>
        public abstract AlgebraExpression Factor();
        #endregion

        /// <summary>
        /// Computes the constant value of the expression.
        /// </summary>
        /// <returns></returns>
        public abstract double? GetConstantValue();
        
        #region Calculus
        /// <summary>
        /// Computes the limit as <paramref name="expression"/> approaches <paramref name="subject"/>.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public abstract AlgebraExpression Limit(AlgebraExpression expression, AlgebraExpression subject, LimitDirection direction = LimitDirection.Both);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="respectTo"></param>
        /// <returns></returns>
        public virtual AlgebraExpression Differentiate(AlgebraExpression respectTo)
        {
            VariableExpression limitVariable = new VariableExpression("h");
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Constructs the primitive function the given expression respect to <paramref name="respectTo"/>.
        /// </summary>
        /// <param name="respectTo"></param>
        /// <returns></returns>
        public abstract AlgebraExpression Integrate(AlgebraExpression respectTo);
        #endregion
    }
}
