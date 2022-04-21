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
public record class FunctionInvocationExpression(string Name, IImmutableList<AlgebraExpression> Arguments) : AlgebraExpression, IEquatable<FunctionInvocationExpression>
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken)) => null;

	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this with
	{
		Arguments = Arguments.Select(a => a.Substitute(variable, replacement)).ToImmutableList()
	};

	public override IEnumerable<AlgebraExpression> Children() => Arguments;

	public virtual bool Equals(FunctionInvocationExpression? other) => other != null && Name == other.Name && Arguments.Zip(other.Arguments, (x, y) => x.Equals(y)).All(i => i);

	public override int GetHashCode() => Arguments.Select(o => o.GetHashCode()).Aggregate(Name.GetHashCode(), (x, y) => x ^ y);

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.Append(Name);
		sb.Append('(');
		sb.Append(String.Join(", ", Arguments));
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
