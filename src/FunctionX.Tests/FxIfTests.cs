using Microsoft.CodeAnalysis.Scripting;

namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxIfTests
{
    [Fact]
    public async Task TestIfFunction_Literal()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("IF(true, 'Yes', \"No I'm fine\")", parameters);

        // Assert
        Assert.Equal("Yes", result);
    }

    [Fact]
    public async Task TestIfFunction_WhenConditionIsFalse_ReturnFalseValue()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("IF(false, \"Yes\", \"No\")", parameters);

        // Assert
        Assert.Equal("No", result);
    }

    [Fact]
    public async Task TestIfFunction_WhenLogical()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("IF(1 > 5, \"hello\", \"world\")", parameters);

        // Assert
        Assert.Equal("world", result);
    }

    [Fact]
    public async Task TestIfFunction_WhenConditionIsNull_ReturnDefaultValue()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();
        var result = await Fx.EvaluateAsync("IF(null, \"Yes\", \"No\")", parameters);

        // Assert
        Assert.Equal("No", result);
    }

    [Fact]
    public async Task TestIfFunction_WhenConditionIsNonBoolean_ReturnDefaultValue()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        var result = await Fx.EvaluateAsync("IF(123, \"Yes\", \"No\")", parameters);

        // Assert
        Assert.Equal("Yes", result);
    }

    [Fact]
    public async Task TestComplexFunction_WithVariables()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
            {
                { "var1", 10 },
                { "var2", 5 },
                { "var3", 20 }
            };

        // Act
        var result = await Fx.EvaluateAsync("IF(@var1 > @var2, CONCAT(\"var1 is greater than var2, and var3 is \", @var3), \"Condition is false\")", parameters);

        // Assert
        Assert.Equal("var1 is greater than var2, and var3 is 20", result);
    }

    [Fact]
    public async Task TestLogical()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
            {
                { "var1", 10 },
                { "var2", 5 }
            };

        // Act
        var result = await Fx.EvaluateAsync("IF(@var1 > @var2, \"var1 is greater than var2.\",\"var2 is greater than var1.\")", parameters);

        // Assert
        Assert.Equal("var1 is greater than var2.", result);
    }
}