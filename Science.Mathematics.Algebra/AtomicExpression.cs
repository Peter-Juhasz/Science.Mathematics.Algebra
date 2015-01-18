using System;

namespace Science.Mathematics.Algebra
{
    public abstract class AtomicExpression : AlgebraExpression
    {
        public override AlgebraExpression Simplify()
        {
            return this;
        }

        public override AlgebraExpression Expand()
        {
            return this;
        }

        public override AlgebraExpression Factor()
        {
            return this;
        }


        public override AlgebraExpression Substitute(AlgebraExpression subject, AlgebraExpression replacement)
        {
            return this == subject ? replacement : subject;
        }
    }
}
