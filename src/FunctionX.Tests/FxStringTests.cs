namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// String 관련 함수 (LEFT, RIGHT, MID, LEN, CONCAT, TRIM)
/// </summary>
public class FxStringTests
{
    [Fact]
    public void TestLeftFunction_WithValidParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "num", 5 } };

        // Act
        var result = Fx.Evaluate("LEFT(str, num)", parameters);

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void TestLeftFunction_WithStringShorterThanLength()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hi" }, { "num", 10 } };

        // Act
        var result = Fx.Evaluate("LEFT(str, num)", parameters);

        // Assert
        Assert.Equal("Hi", result);
    }

    [Fact]
    public void TestRightFunction_WithValidParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "num", 6 } };

        // Act
        var result = Fx.Evaluate("RIGHT(str, num)", parameters);

        // Assert
        Assert.Equal("world!", result);
    }

    [Fact]
    public void TestRightFunction_WithStringShorterThanLength()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hi" }, { "num", 10 } };

        // Act
        var result = Fx.Evaluate("RIGHT(str, num)", parameters);

        // Assert
        Assert.Equal("Hi", result);
    }

    [Fact]
    public void TestMidFunction_WithValidParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "start", 8 }, { "length", 5 } };

        // Act
        var result = Fx.Evaluate("MID(str, start, length)", parameters);

        // Assert
        Assert.Equal("world", result);
    }

    [Fact]
    public void TestMidFunction_WithStartPositionOutOfBounds()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "start", 20 }, { "length", 5 } };

        // Act
        var result = Fx.Evaluate("MID(str, start, length)", parameters);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void TestLenFunction_WithValidString()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" } };

        // Act
        var result = Fx.Evaluate("LEN(str)", parameters);

        // Assert
        Assert.Equal(13, result);
    }

    [Fact]
    public void TestLenFunction_WithEmptyString()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "" } };

        // Act
        var result = Fx.Evaluate("LEN(str)", parameters);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void TestConcatFunction_WithMultipleStrings()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
        {
            { "str1", "Hello" },
            { "str2", ", " },
            { "str3", "world" },
            { "str4", "!" }
        };

        // Act
        var result = Fx.Evaluate("CONCAT(str1, str2, str3, str4)", parameters);

        // Assert
        Assert.Equal("Hello, world!", result);
    }

    [Fact]
    public void TestConcatFunction_WithStringsAndNumbers()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
        {
            { "str1", "The answer is " },
            { "num1", 42 }
        };

        // Act
        var result = Fx.Evaluate("CONCAT(str1, num1)", parameters);

        // Assert
        Assert.Equal("The answer is 42", result);
    }
}
