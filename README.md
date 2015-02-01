# Science.Mathematics.Algebra

Computer Algebra System (CAS) implemented in .NET/C# using Roslyn as a model.

```C#
VariableExpression x = "x";
AlgebraExpression expression = (x ^ 2) + 3 * x + 5;

var differentiated = expression.Differentiate(x);
```

## Extensibility for simplification logic

An example for a simplifier which denotes that power expressions like `x ^ 1` can be simplified to `x`:

```C#
public sealed class ExponentOneSimplifier : ISimplifier<PowerExpression>
{
    public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
    {
        if (expression.Exponent.GetConstantValue(cancellationToken) == 1)
            return expression.Base;

        return expression;
    }
}
```