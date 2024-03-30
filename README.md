# FunctionX

The FunctionX library implements Excel-like formula features in C#. It allows users to manipulate data in ways similar to Excel formulas. The library utilizes the NCalc library for evaluating expressions and supports complex data types to perform various operations.

## Features

The FunctionX library offers the following features:


## Usage

To use the FunctionX library:

1. Call the `Fx.Evaluate` method to evaluate a formula.
2. Pass the formula string and a dictionary of parameters.

### Examples

Here are some examples of using the FunctionX library:

```csharp
// Basic mathematical operation
var result = await Fx.EvaluateAsync("1 + 2 + 3");

// Calculate the average value in an array
var parameters = new Dictionary<string, object?>
{
    { "myArray", new object[] {1, 2, 3, 4, 5} }
};
var average = await Fx.EvaluateAsync("AVERAGE(@myArray)", parameters);

// Conditional logic
var parameters = new Dictionary<string, object?>
{
    { "value1", 10 },
    { "value2", 20 }
};
var ifResult = await Fx.EvaluateAsync("IF(@value1 > @value2, \"Yes\", \"No\")", parameters);
```

## Implemented Functions
| Function Name | Description |
|---------------|-------------|
| `SUM` | Calculates the sum of numeric values. |
| `AVERAGE` | Calculates the average of numeric values. |
| `MAX` | Finds the maximum value among numeric values. |
| `MIN` | Finds the minimum value among numeric values. |
| `COUNT` | Counts the number of numeric values. |
| `COUNTA` | Counts the number of non-empty values. |
| `AND` | Returns true if all values are true. |
| `OR` | Returns true if at least one value is true. |
| `NOT` | Returns true if the value is false. |
| `XOR` | Returns true if an odd number of values are true. |
| `IF` | Returns one of two values depending on a condition. |
| `IFS` | Returns the result of the first true condition among multiple conditions. |
| `SWITCH` | Returns a value based on given cases or provides a default value. |
| `CONCAT` | Concatenates string values. |
| `LEFT` | Returns a specified number of characters from the beginning of a string. |
| `RIGHT` | Returns a specified number of characters from the end of a string. |
| `MID` | Returns a specified number of characters from a string starting at a specified position. |
| `TRIM` | Removes leading and trailing whitespace from a string. |
| `UPPER` | Converts a string to uppercase. |
| `LOWER` | Converts a string to lowercase. |
| `PROPER` | Converts the first letter of each word in a string to uppercase. |
| `REPLACE` | Replaces occurrences of a specified substring within a string with another substring. |
| `LEN` | Returns the length of a string. |
| `INDEX` | Returns the value at a specified position in an array or dictionary. |
| `VLOOKUP` | Searches for a value in the first column of a range and returns a value in the same row from a specified column. |
| `UNIQUE` | Returns an array of unique values with duplicates removed. |

## Installation

This library is not currently available via NuGet. To use the library, you will need to download the source code directly and include it in your project.

## Requirements

- netstandard2.1

## Contributing

FunctionX is an open-source project. If you would like to contribute, please send a pull request or file an issue.

## License

The FunctionX library is provided under the MIT license.