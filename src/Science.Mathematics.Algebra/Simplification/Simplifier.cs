﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies <see cref="AlgebraExpression"/>s using all the simplifiers found in the library.
/// </summary>
public static class Simplifier
{
	/// <summary>
	/// Simplifies an <see cref="AlgebraExpression"/> as much as it can.
	/// </summary>
	/// <param name="expression"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public static AlgebraExpression Simplify(this AlgebraExpression expression, CancellationToken cancellationToken = default)
	{
		AlgebraExpression simplified = expression;
		AlgebraExpression lastResult;

		do
		{
			lastResult = simplified;

			// simplify by kind
			simplified = SimplifySpecificKind(simplified, cancellationToken);
		} while (!lastResult.Equals(simplified));

		return simplified;
	}


	private static AlgebraExpression SimplifySpecificKind(AlgebraExpression expression, CancellationToken cancellationToken)
	{
		AlgebraExpression simplified = expression;
		AlgebraExpression lastResult;

		// find applicable simplifiers
		IReadOnlyCollection<object> simplifiers = GetSimplifiers(expression);

		do
		{
			lastResult = simplified;

			// query each simplifier
			foreach (var simplifier in simplifiers)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var original = simplified;

				MethodInfo method = simplifier.GetType().GetRuntimeMethod(nameof(ISimplifier<AlgebraExpression>.Simplify), new[] { expression.GetType(), typeof(CancellationToken) });
				simplified = method.Invoke(simplifier, new object[] { simplified, cancellationToken }) as AlgebraExpression;

				// trace
				if (!original.Equals(simplified))
					Debug.WriteLine($"Simplified '{original}' to '{simplified}' using '{simplifier}'");

				// stop if not the same kind of expression
				if (lastResult.GetType() != simplified.GetType())
					return simplified;
			}
		} while (!lastResult.Equals(simplified));

		return simplified;
	}

	private static IEnumerable<Type> FindSimplifiers(Type expressionType) => typeof(ISimplifier<>).GetTypeInfo().Assembly.GetTypes()
		.Where(t =>
			t.GetTypeInfo().ImplementedInterfaces
				.Where(i => i.IsConstructedGenericType)
				.Where(i => i.GetGenericTypeDefinition() == typeof(ISimplifier<>))
				.Any(i => i.GenericTypeArguments[0].GetTypeInfo().IsAssignableFrom(expressionType.GetTypeInfo()))
		)
	;

	private static IReadOnlyCollection<object> GetSimplifiers(AlgebraExpression expression) => FindSimplifiers(expression.GetType())
		.Select(Activator.CreateInstance)
		.ToList();
}
