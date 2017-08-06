using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents an absolute value expression.
    /// </summary>
    public class InfinityExpression : AlgebraExpression, IEquatable<InfinityExpression>
    {
        public InfinityExpression()
        {
        }
        
        
        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            return null;
        }


        public override AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement)
        {
            return this;
        }

        public override IEnumerable<AlgebraExpression> Children()
        {
            yield break;
        }


        #region Immutability
        public AbsoluteValueExpression WithExpression(AlgebraExpression newExpression)
        {
            return ExpressionFactory.AbsoluteValue(newExpression);
        }
        #endregion


        public override string ToString()
        {
            return $"∞";
        }

        public bool Equals(InfinityExpression other)
        {
            return other != null;
        }

        public override int GetHashCode()
        {
            return Int32.MaxValue;
        }

        public override bool Equals(object obj)
        {
            return obj is InfinityExpression;
        }
    }
    
    public static partial class ExpressionFactory
    {
        public static InfinityExpression Infinity()
        {
            return new InfinityExpression();
        }
    }
}
