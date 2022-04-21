using System;

namespace Science.Mathematics.Algebra;

[AttributeUsage(AttributeTargets.Class)]
public class FunctionNameAttribute : Attribute
{
	public FunctionNameAttribute(string name)
	{
		Name = name;
	}

	public string Name { get; }
}
