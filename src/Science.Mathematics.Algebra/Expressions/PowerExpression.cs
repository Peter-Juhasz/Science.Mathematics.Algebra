using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Represents a power expression.
/// </summary>
public record class PowerExpression(AlgebraExpression Base, AlgebraExpression Exponent) : AlgebraExpression
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default)
	{
		var @base = Base.GetConstantValue(cancellationToken);
		var exponent = Exponent.GetConstantValue(cancellationToken);

		if (@base != null && exponent != null)
			return (decimal)Math.Pow((double)@base.Value, (double)exponent.Value);

		return null;
	}


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this with
	{
		Base = Base.Substitute(variable, replacement),
		Exponent = Exponent.Substitute(variable, replacement)
	};

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return Base;
		yield return Exponent;
	}


	internal AlgebraExpression Normalize()
	{
		if (Exponent == One)
			return Base;

		return this;
	}


	public override int GetHashCode() => HashCode.Combine(Base, Exponent);

	public override string ToString()
	{
		var result = new StringBuilder();

		var baseNeedsParenthesis = Base is ExpressionList;
		if (baseNeedsParenthesis)
			result.Append('(');

		result.Append(Base);

		if (baseNeedsParenthesis)
			result.Append(')');

		result.Append(' ');
		result.Append('^');
		result.Append(' ');

		var exponentNeedsParenthesis = Exponent is ExpressionList;
		if (exponentNeedsParenthesis)
			result.Append('(');

		result.Append(Exponent);

		if (exponentNeedsParenthesis)
			result.Append(')');

		return result.ToString();
	}
}

public static partial class AlgebraExpressionExtensions
{
	public static PowerExpression AsPower(this AlgebraExpression expression) => expression switch
	{
		PowerExpression power => power,
		_ => Exponentiate(expression, One)
	};
}

public static partial class ExpressionFactory
{
	public static PowerExpression Exponentiate(AlgebraExpression @base, AlgebraExpression exponent) => new(@base, exponent);
	public static PowerExpression Root(AlgebraExpression @base, AlgebraExpression exponent) => Exponentiate(@base, Divide(NumberExpression.One, exponent));
	public static PowerExpression Square(AlgebraExpression expression) => Exponentiate(expression, Number(2));
	public static PowerExpression SquareRoot(AlgebraExpression expression) => Exponentiate(expression, Divide(NumberExpression.One, Number(2)));
}
