# Science.Mathematics.Algebra

Computer Algebra System (CAS) implemented in .NET/C# using Roslyn as a model.



```C#
VariableExpression x = "x";
AlgebraExpression expression = (x ^ 2) + 3 * x + 5;

var differentiated = expression.Differentiate(x);
```