using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

public record class ProductExpressionList : ExpressionList, IEquatable<ProductExpressionList>
{
	public ProductExpressionList(IImmutableList<AlgebraExpression> Terms)
		: base(Terms)
	{
	}

	public override decimal? GetConstantValue(CancellationToken cancellationToken = default) => Terms.Select(t => t.GetConstantValue()).Product(v => v);

	
	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this with
	{
		Terms = Terms
			.Select(t => t == variable ? replacement : variable)
			.Select(t => t.Substitute(variable, replacement))
			.ToImmutableList()
	};

	public AlgebraExpression GetNumerator() => Product(
		Terms
			.Where(t => t.AsPower().Exponent.AsProduct().Terms.OfType<NumberExpression>().Select(n => n.Value).Product() >= 0)
			.ToList()
	).Normalize();

	public AlgebraExpression GetDenominator() => Product((
		from t in Terms
		let pow = t.AsPower()
		let prod = t.AsPower().Exponent.AsProduct()
		let st = prod.Terms.OfType<NumberExpression>()
		where st.Select(n => n.Value).Product() < 0
		let negative = st.Where(n => n.Value < 0).First()
		let index = prod.Terms.IndexOf(negative)
		select (pow with { Exponent = (prod with { Terms = prod.Terms.RemoveAt(index).Insert(index, -negative.Value) }).Normalize() }).Normalize()
	).ToList()).Normalize();

	public virtual bool Equals(ProductExpressionList? other) => other != null && Terms.Zip(other.Terms, (x, y) => x.Equals(y)).All(i => i);

	internal AlgebraExpression Normalize()
	{
		if (Terms.Count == 1)
			return Terms.Single();

		return this;
	}

	public override int GetHashCode() => Terms.Select(o => o.GetHashCode()).Aggregate((x, y) => x ^ y);

	public override string ToString() => String.Join(" * ", Terms
		.Select(t => NeedsParenthesis(t) ? $"({t})" : t.ToString())
	);

	private static bool NeedsParenthesis(AlgebraExpression expression) => expression is SumExpressionList;
}

public static partial class AlgebraExpressionExtensions
{
	internal static ProductExpressionList AsProduct(this AlgebraExpression expression)
	{
		if (expression is ProductExpressionList product)
			return product;

		return Product(expression);
	}
}

public static partial class ExpressionFactory
{
	public static ProductExpressionList Negate(AlgebraExpression expression) => Multiply(NumberExpression.MinusOne, expression);

	public static ProductExpressionList Multiply(AlgebraExpression left, AlgebraExpression right) => new ProductExpressionList(ImmutableList.Create(left, right));
	public static ProductExpressionList Divide(AlgebraExpression left, AlgebraExpression right) => Multiply(left, Exponentiate(right, NumberExpression.MinusOne));

	public static PowerExpression Reciprocal(AlgebraExpression expression) => Exponentiate(expression, NumberExpression.MinusOne);

	public static ProductExpressionList Product(IReadOnlyCollection<AlgebraExpression> terms) => new ProductExpressionList(terms.ToImmutableList());
	public static ProductExpressionList Product(params AlgebraExpression[] terms) => Product(terms as IReadOnlyCollection<AlgebraExpression>);
}
