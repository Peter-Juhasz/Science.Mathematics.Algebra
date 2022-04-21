using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

public class ProductExpressionList : ExpressionList, IEquatable<ProductExpressionList>
{
	public ProductExpressionList(IReadOnlyCollection<AlgebraExpression> terms)
		: base(terms)
	{ }


	public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken)) => this.Terms.Select(t => t.GetConstantValue()).Product(v => v);


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this.WithTerms(
			this.Terms
				.Select(t => t == variable ? replacement : variable)
				.Select(t => t.Substitute(variable, replacement))
				.ToImmutableList()
		);


	#region Immutability
	public ProductExpressionList Multiply(AlgebraExpression expression) => expression is ProductExpressionList
			? ExpressionFactory.Product(this.Terms.Concat((expression as ProductExpressionList).Terms).ToImmutableList())
			: ExpressionFactory.Multiply(this, expression)
		;

	public ProductExpressionList Divide(AlgebraExpression expression) => expression is ProductExpressionList
			? ExpressionFactory.Product(this, ExpressionFactory.Reciprocal(expression))
			: ExpressionFactory.Divide(this, expression)
		;

	public ProductExpressionList WithTerms(IImmutableList<AlgebraExpression> newTerms) => ExpressionFactory.Product(newTerms);
	#endregion


	public AlgebraExpression GetNumerator() => Product(
			this.Terms
				.Where(t => t.AsPower().Exponent.AsProduct().Terms.OfType<NumberExpression>().Select(n => n.Value).Product() >= 0)
				.ToList()
		).Normalize();

	public AlgebraExpression GetDenominator() => Product((
			from t in this.Terms
			let pow = t.AsPower()
			let prod = t.AsPower().Exponent.AsProduct()
			let st = prod.Terms.OfType<NumberExpression>()
			where st.Select(n => n.Value).Product() < 0
			let negative = st.Where(n => n.Value < 0).First()
			let index = prod.Terms.IndexOf(negative)
			select pow.WithExponent(prod.WithTerms(prod.Terms.RemoveAt(index).Insert(index, -negative.Value)).Normalize()).Normalize()
		).ToList()).Normalize();


	internal AlgebraExpression Normalize()
	{
		if (this.Terms.Count == 1)
			return this.Terms.Single();

		return this;
	}


	public bool Equals(ProductExpressionList other)
	{
		if (Object.ReferenceEquals(other, null)) return false;

		if (this.Terms.Count != other.Terms.Count)
			return false;

		return this.Terms.Zip(other.Terms, (x, y) => x.Equals(y)).All(b => b);
	}
	public override bool Equals(object obj) => this.Equals(obj as ProductExpressionList);

	public override int GetHashCode() => this.Terms.Select(o => o.GetHashCode()).Aggregate((x, y) => x ^ y);

	public override string ToString() => String.Join(" * ", this.Terms
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
