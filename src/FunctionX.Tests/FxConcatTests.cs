namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxConcatTests
{
    [Fact]
    public void TestConcatFunction_WithStrings()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str1", "Hello, " }, { "str2", "world!" } };

        // Act
        var result = Fx.Evaluate("CONCAT(str1, str2)", parameters);

        // Assert
        Assert.Equal("Hello, world!", result);
    }

    [Fact]
    public void TestConcatFunction_WithNumbers()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "num1", 123 }, { "num2", 456 } };

        // Act
        var result = Fx.Evaluate("CONCAT(num1, num2)", parameters);

        // Assert
        Assert.Equal("123456", result);
    }

    [Fact]
    public void TestConcatFunction_WithNull()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str1", "Hello, " }, { "str2", null } };

        // Act
        var result = Fx.Evaluate("CONCAT(str1, str2)", parameters);

        // Assert
        Assert.Equal("Hello, ", result);
    }

    [Fact]
    public void TestConcatFunction_MultipleParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
            {
                { "str1", "Hello, " },
                { "str2", "world!" },
                { "num1", 123 },
                { "num2", 456 }
            };

        // Act
        var result = Fx.Evaluate("CONCAT(str1, str2, num1, num2)", parameters);

        // Assert
        Assert.Equal("Hello, world!123456", result);
    }

    [Fact]
    public void TestConcatFunction_WithIndexResult()
    {
        // Arrange
        var range = new List<Dictionary<string, object?>>
        {
            new() { { "Name", "John" }, { "Age", 30 } },
            new() { { "Name", "Alice" }, { "Age", 25 } }
        };
        var parameters = new Dictionary<string, object?>
        {
            { "range", range },
            { "rowIndex", 1 },
            { "columnIndex", 0 }
        };

        // Act
        var result = Fx.Evaluate("CONCAT(INDEX(range, rowIndex, columnIndex), ' is ', INDEX(range, rowIndex, columnIndex+1))", parameters);

        // Assert
        Assert.Equal("John is 30", result);
    }
}
