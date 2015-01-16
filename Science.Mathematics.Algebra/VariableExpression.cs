using System;

namespace Science.Mathematics.Algebra
{
    public class VariableExpression : AtomicExpression, IEquatable<VariableExpression>
    {
        public VariableExpression(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("Variable name can not be a zero-length string.", "name");

            if (!Char.IsLetter(name[0]))
                throw new ArgumentException("Variable name must start with a letter.", "name");

            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        public string Name { get; private set; }


        public override double? GetConstantValue()
        {
            return null;
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


        public override string ToString()
        {
            return this.Name;
        }

        public bool Equals(VariableExpression other)
        {
            if (other == null) return false;

            return this.Name == other.Name;
        }
        
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as VariableExpression);
        }
    }
}
