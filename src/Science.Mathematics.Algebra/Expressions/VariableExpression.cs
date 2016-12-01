using System;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents a variable.
    /// </summary>
    public class VariableExpression : AtomicExpression, IEquatable<VariableExpression>
    {
        public VariableExpression(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("Variable name can not be a zero-length string.", nameof(name));

            if (!Char.IsLetter(name[0]))
                throw new ArgumentException("Variable name must start with a letter.", nameof(name));

            this.Name = name;
        }
        public VariableExpression(char name)
            : this(name.ToString())
        { }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            return null;
        }


        public override AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement)
        {
            return this.Name == variable.Name ? replacement : this;
        }


        #region Conversions
        public static implicit operator VariableExpression(char ch)
        {
            return new VariableExpression(ch.ToString());
        }

        public static implicit operator VariableExpression(string name)
        {
            return new VariableExpression(name);
        }
        #endregion


        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public bool Equals(VariableExpression other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            return this.Name == other.Name;
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as VariableExpression);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public static partial class ExpressionFactory
    {
        public static VariableExpression Variable(string name)
        {
            return new VariableExpression(name);
        }
        public static VariableExpression Variable(char name)
        {
            return new VariableExpression(name);
        }
    }
}
