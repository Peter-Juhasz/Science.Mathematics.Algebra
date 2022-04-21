using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Mathematics.Algebra;

internal static class Extensions
{
	public static double Product(this IEnumerable<double> source) => source.Product(_ => _);
	public static double? Product(this IEnumerable<double?> source) => source.Product(_ => _);

	public static double Product<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector) => source.Select(selector).Aggregate((x, y) => x * y);
	public static double? Product<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector) => source.Select(selector).Aggregate((x, y) => x * y);
}
