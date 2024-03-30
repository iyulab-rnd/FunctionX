namespace FunctionX.Tests;

public class FxCountATests
{
    [Fact]
    public async Task TestEvaluateCountAWithConstants()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("COUNTA(1, 2, \"text\", null, \"\")", results);

        // Assert
        Assert.Equal(4.0, Convert.ToDouble(result)); // null은 제외하고 계산
    }

    [Fact]
    public async Task TestEvaluateCountAWithVariable()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "countAValues", new object?[] {1, 2, 3, "text", null} }
        };

        // Act
        var result = await Fx.EvaluateAsync("COUNTA(@countAValues)", results);

        // Assert
        Assert.Equal(4.0, Convert.ToDouble(result)); // null은 제외하고 계산
    }

    [Fact]
    public async Task TestEvaluateCountAWithMixedTypesIncludingEmptyStrings()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "mixedValues", new object?[] {1, null, "apple", "", 5} }
        };

        // Act
        var result = await Fx.EvaluateAsync("COUNTA(@mixedValues)", results);

        // Assert
        Assert.Equal(4.0, Convert.ToDouble(result)); // null은 제외하지만, 빈 문자열은 포함하여 계산
    }

    [Fact]
    public async Task TestEvaluateCountAWithAllStringsIncludingEmpty()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "stringValues", new object[] {"apple", "banana", "", "cherry"} }
        };

        // Act
        var result = await Fx.EvaluateAsync("COUNTA(@stringValues)", results);

        // Assert
        // 빈 문자열 포함, 모든 문자열 값 계산
        Assert.Equal(4.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateCountAWithEmptyArray()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "emptyArray", Array.Empty<object>() }
        };

        // Act
        var result = await Fx.EvaluateAsync("COUNTA(@emptyArray)", results);

        // Assert
        // 빈 배열, 즉 값이 없으므로 0 반환
        Assert.Equal(0.0, Convert.ToDouble(result));
    }
}
