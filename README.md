# Science.Mathematics.Algebra

Computer Algebra System (CAS) implemented in **.NET Standard 1.6** using Roslyn as a model.

You can create algebra expressions using `ExpressionFactory`, by implicit conversions (numbers and symbols) and by combining them using the basic operators.
```C#
using static ExpressionFactory;
```

Let's do some symbolic computation:
```C#
var x = Symbol('x');
```

You can perform basic operations to build new expressions using the built-in operators:
```C#
var expression = (x ^ 2) + 3 * x + 5;
```

Differentiate the expression above by `x`:
```C#
var differentiated = expression.Differentiate(x); // d/dx (x ^ 2) + 3 * x + 5
```

This gives you a new kind of expression not the result yet. If you want to evaluate it, call `Simplify` which is going to return the most simple form (eliminating the zero constant from the end):
```C#
var result = differentiated.Simplify(); // 2 * x + 3
```

Or you can also convert the differentiation expression to a limit:
```C#
var limit = differentiated.ToLimit();
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
