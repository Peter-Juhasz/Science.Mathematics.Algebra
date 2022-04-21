using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents a function invocation expression.
/// </summary>
public class FunctionInvocationExpression : AlgebraExpression, IEquatable<FunctionInvocationExpression>
{
	public FunctionInvocationExpression(string name, IImmutableList<AlgebraExpression> arguments)
	{
		if (name == null)
			throw new ArgumentNullException(nameof(name));

		if (arguments == null)
			throw new ArgumentNullException(nameof(arguments));

		this.Name = name;
		this.Arguments = arguments;
	}


	/// <summary>
	/// Gets the name of the invoked function.
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Gets the arguments of the invocation.
	/// </summary>
	public IImmutableList<AlgebraExpression> Arguments { get; private set; }


	public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken)) => null;

	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this.WithArguments(this.Arguments.Select(a => a.Substitute(variable, replacement)).ToImmutableList());

	public override IEnumerable<AlgebraExpression> Children() => this.Arguments;


	#region Immutability
	public FunctionInvocationExpression WithName(string name) => ExpressionFactory.Invoke(name, this.Arguments);

	public FunctionInvocationExpression WithArguments(IImmutableList<AlgebraExpression> arguments) => ExpressionFactory.Invoke(this.Name, arguments);
	#endregion


	public bool Equals(FunctionInvocationExpression other)
	{
		if (Object.ReferenceEquals(other, null)) return false;

		if (this.Name != other.Name)
			return false;

		if (this.Arguments.Count != other.Arguments.Count)
			return false;

		return this.Arguments.Zip(other.Arguments, (x, y) => x.Equals(y)).All(b => b);
	}
	public override bool Equals(object obj) => this.Equals(obj as FunctionInvocationExpression);

	public override int GetHashCode() => this.Arguments.Select(o => o.GetHashCode()).Aggregate(this.Name.GetHashCode(), (x, y) => x ^ y);

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append(this.Name);
		sb.Append('(');
		sb.Append(String.Join(", ", this.Arguments));
		sb.Append(')');

		return sb.ToString();
	}
}

public static partial class ExpressionFactory
{
	public static FunctionInvocationExpression Invoke(string name, IReadOnlyList<AlgebraExpression> arguments)
	{
		var type = typeof(FunctionInvocationExpression).GetTypeInfo().Assembly.GetTypes()
			.Select(t => t.GetTypeInfo())
			.Where(t => !t.IsSpecialName)
			.Where(t => t.IsClass)
			.Where(t => !t.IsAbstract)
			.Where(t => t.IsSubclassOf(typeof(FunctionInvocationExpression)))
			.SingleOrDefault(t => t.GetCustomAttributes<FunctionNameAttribute>().Any(a => a.Name == name));

		if (type != null)
		{
			var constructors = type.DeclaredConstructors;

			if (constructors.Any(c => c.GetParameters().Count() == 1 && c.GetParameters().Single().ParameterType == typeof(IImmutableList<AlgebraExpression>)))
				return Activator.CreateInstance(type.AsType(), arguments.ToImmutableList()) as FunctionInvocationExpression;

			if (constructors.Any(c => c.GetParameters().Count() == arguments.Count && c.GetParameters().All(p => p.ParameterType == typeof(AlgebraExpression))))
				return Activator.CreateInstance(type.AsType(), args: arguments.ToArray()) as FunctionInvocationExpression;
		}

		return new FunctionInvocationExpression(name, arguments.ToImmutableList());
	}
	public static FunctionInvocationExpression Invoke(string name, params AlgebraExpression[] arguments) => Invoke(name, arguments as IReadOnlyList<AlgebraExpression>);
}
