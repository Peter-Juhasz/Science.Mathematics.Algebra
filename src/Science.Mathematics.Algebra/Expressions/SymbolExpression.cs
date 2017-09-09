using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents a variable.
    /// </summary>
    public class SymbolExpression : AtomicExpression, IEquatable<SymbolExpression>
    {
        public SymbolExpression(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("Variable name can not be a zero-length string.", nameof(name));

            if (!Char.IsLetter(name[0]))
                throw new ArgumentException("Variable name must start with a letter.", nameof(name));

            this.Name = name;
        }
        public SymbolExpression(char name)
            : this(name.ToString())
        { }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default)
        {
            return null;
        }


        public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement)
        {
            return this.Name == variable.Name ? replacement : this;
        }

        public override IEnumerable<PatternMatch> MatchTo(AlgebraExpression expression, CancellationToken cancellationToken = default)
        {
            yield return new PatternMatch(this, this, expression);
        }

        #region Conversions
        public static implicit operator SymbolExpression(char ch)
        {
            return new SymbolExpression(ch.ToString());
        }

        public static implicit operator SymbolExpression(string name)
        {
            return new SymbolExpression(name);
        }
        #endregion


        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public bool Equals(SymbolExpression other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            return this.Name == other.Name;
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as SymbolExpression);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public static partial class ExpressionFactory
    {
        public static SymbolExpression Symbol(string name)
        {
            return new SymbolExpression(name);
        }
        public static SymbolExpression Symbol(char name)
        {
            return new SymbolExpression(name);
        }
    }
}
