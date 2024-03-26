# FunctionX

The FunctionX library implements Excel-like formula features in C#. It allows users to manipulate data in ways similar to Excel formulas. The library utilizes the NCalc library for evaluating expressions and supports complex data types to perform various operations.

## Features

The FunctionX library offers the following features:

- Mathematical and statistical operations: `SUM`, `AVERAGE`, `MIN`, `MAX`, etc.
- String operations: `CONCAT`, `LEFT`, `RIGHT`, `MID`, `LEN`, `TRIM`, etc.
- Logical operations: Support for the `IF` function.
- Range and array handling: `INDEX`, `COUNT`, etc.
- Variable and parameter substitution: Dynamic value calculation based on parameters passed to functions.

## Usage

To use the FunctionX library:

1. Call the `Fx.Evaluate` method to evaluate a formula.
2. Pass the formula string and a dictionary of parameters.

### Examples

Here are some examples of using the FunctionX library:

```csharp
// Basic mathematical operation
var result = Fx.Evaluate("1 + 2 + 3", new Dictionary<string, object?>());

// Calculate the average value in an array
var results = new Dictionary<string, object?>
{
    { "myArray", new object[] {1, 2, 3, 4, 5} }
};
var average = Fx.Evaluate("AVERAGE(myArray)", results);

// Conditional logic
var conditionResults = new Dictionary<string, object?>
{
    { "value1", 10 },
    { "value2", 20 }
};
var ifResult = Fx.Evaluate("IF(value1 > value2, 'Yes', 'No')", conditionResults);
```

## Installation

This library is not currently available via NuGet. To use the library, you will need to download the source code directly and include it in your project.

## Requirements

- .NET 6 or higher
- NCalc library

## Contributing

FunctionX is an open-source project. If you would like to contribute, please send a pull request or file an issue.

## License

The FunctionX library is provided under the MIT license.