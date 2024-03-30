namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// String 관련 함수 (LEFT, RIGHT, MID, LEN, CONCAT, TRIM)
/// </summary>
public class FxStringTests
{
    [Fact]
    public async Task TestLeftFunction_WithValidParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "num", 5 } };

        // Act
        var result = await Fx.EvaluateAsync("LEFT(@str, @num)", parameters);

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public async Task TestLeftFunction_WithStringShorterThanLength()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hi" }, { "num", 10 } };

        // Act
        var result = await Fx.EvaluateAsync("LEFT(@str, @num)", parameters);

        // Assert
        Assert.Equal("Hi", result);
    }

    [Fact]
    public async Task TestRightFunction_WithValidParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "num", 6 } };

        // Act
        var result = await Fx.EvaluateAsync("RIGHT(@str, @num)", parameters);

        // Assert
        Assert.Equal("world!", result);
    }

    [Fact]
    public async Task TestRightFunction_WithStringShorterThanLength()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hi" }, { "num", 10 } };

        // Act
        var result = await Fx.EvaluateAsync("RIGHT(@str, @num)", parameters);

        // Assert
        Assert.Equal("Hi", result);
    }

    [Fact]
    public async Task TestMidFunction_WithValidParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "start", 8 }, { "length", 5 } };

        // Act
        var result = await Fx.EvaluateAsync("MID(@str, @start, @length)", parameters);

        // Assert
        Assert.Equal("world", result);
    }

    [Fact]
    public async Task TestMidFunction_WithStartPositionOutOfBounds()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" }, { "start", 20 }, { "length", 5 } };

        // Act
        var result = await Fx.EvaluateAsync("MID(@str, @start, @length)", parameters);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public async Task TestLenFunction_WithValidString()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "Hello, world!" } };

        // Act
        var result = await Fx.EvaluateAsync("LEN(@str)", parameters);

        // Assert
        Assert.Equal(13, result);
    }

    [Fact]
    public async Task TestLenFunction_WithEmptyString()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "" } };

        // Act
        var result = await Fx.EvaluateAsync("LEN(@str)", parameters);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task TestConcatFunction_WithMultipleStrings()
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
        var result = await Fx.EvaluateAsync("CONCAT(@str1, @str2, @str3, @str4)", parameters);

        // Assert
        Assert.Equal("Hello, world!", result);
    }

    [Fact]
    public async Task TestConcatFunction_WithStringsAndNumbers()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
        {
            { "str1", "The answer is " },
            { "num1", 42 }
        };

        // Act
        var result = await Fx.EvaluateAsync("CONCAT(@str1, @num1)", parameters);

        // Assert
        Assert.Equal("The answer is 42", result);
    }

    [Fact]
    public async Task TestTrimFunction_WithLeadingAndTrailingSpaces()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "  Hello, world!  " } };

        // Act
        var result = await Fx.EvaluateAsync("TRIM(@str)", parameters);

        // Assert
        Assert.Equal("Hello, world!", result);
    }

    [Fact]
    public async Task TestUpperFunction_WithLowerCaseString()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "hello, world!" } };

        // Act
        var result = await Fx.EvaluateAsync("UPPER(@str)", parameters);

        // Assert
        Assert.Equal("HELLO, WORLD!", result);
    }

    [Fact]
    public async Task TestLowerFunction_WithUpperCaseString()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "HELLO, WORLD!" } };

        // Act
        var result = await Fx.EvaluateAsync("LOWER(@str)", parameters);

        // Assert
        Assert.Equal("hello, world!", result);
    }

    [Fact]
    public async Task TestProperFunction_WithMixedCaseString()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { { "str", "hELLO, wORLD!" } };

        // Act
        var result = await Fx.EvaluateAsync("PROPER(@str)", parameters);

        // Assert
        Assert.Equal("Hello, World!", result);
    }

    [Fact]
    public async Task TestReplaceFunction_WithValidParameters()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
        {
            { "str", "Hello, world!" },
            { "oldStr", "world" },
            { "newStr", "everyone" }
        };

        // Act
        var result = await Fx.EvaluateAsync("REPLACE(@str, @oldStr, @newStr)", parameters);

        // Assert
        Assert.Equal("Hello, everyone!", result);
    }

}
