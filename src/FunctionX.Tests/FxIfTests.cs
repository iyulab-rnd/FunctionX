namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxIfTests
{
    [Fact]
    public void TestIfFunction_WhenConditionIsTrue_ReturnTrueValue()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = Fx.Evaluate("IF(true, 'Yes', 'No')", parameters);

        // Assert
        Assert.Equal("Yes", result);
    }

    [Fact]
    public void TestIfFunction_WhenConditionIsFalse_ReturnFalseValue()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = Fx.Evaluate("IF(false, 'Yes', 'No')", parameters);

        // Assert
        Assert.Equal("No", result);
    }

    [Fact]
    public void TestIfFunction_WhenConditionIsNull_ReturnDefaultValue()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = Fx.Evaluate("IF(null, 'Yes', 'No')", parameters);

        // Assert
        Assert.Equal("No", result);
    }

    [Fact]
    public void TestIfFunction_WhenConditionIsNonBoolean_ReturnDefaultValue()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = Fx.Evaluate("IF(123, 'Yes', 'No')", parameters);

        // Assert
        Assert.Equal("No", result);
    }

    [Fact]
    public void TestComplexFunction_WithVariables()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
            {
                { "var1", 10 },
                { "var2", 5 },
                { "var3", 20 }
            };

        // Act
        var result = Fx.Evaluate("IF(var1 > var2, CONCAT('var1 is greater than var2, and var3 is ', var3), 'Condition is false')", parameters);

        // Assert
        Assert.Equal("var1 is greater than var2, and var3 is 20", result);
    }
}