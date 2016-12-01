using System;

namespace Science.Mathematics.Algebra
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FunctionNameAttribute : Attribute
    {
        public FunctionNameAttribute(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
