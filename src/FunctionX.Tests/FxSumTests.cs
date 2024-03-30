namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxSumTests
{
    [Fact]
    public async Task TestEvaluateSum()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("1 + 3", results);

        // Assert
        Assert.Equal(4.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateSumValues()
    {
        // Arrange
        var value1 = 1;
        var value3 = 3;
        var results = new Dictionary<string, object?>()
        {
            { "value1", value1 },
            { "value2", value3 }
        };

        // Act
        var result = await Fx.EvaluateAsync("1 + 3 + @value1 + @value2", results);

        var v = 1.0 + 3 + value1 + value3;
        // Assert
        Assert.Equal(v, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateSumValuesWithFunc()
    {
        // Arrange
        var value1 = 1;
        var value3 = 3;
        var results = new Dictionary<string, object?>()
        {
            { "value1", value1 },
            { "value2", value3 }
        };

        // Act
        var result = await Fx.EvaluateAsync("1 + 3 + @value1 + @value2 + SUM(5, 1)", results);

        var v = 1.0 + 3 + value1 + value3 + 5 + 1;
        // Assert
        Assert.Equal(v, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateSumWithConstants()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("SUM(1, 3)", results);

        // Assert
        Assert.Equal(4.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateSumWithVariable()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "addValue", 10 }
        };

        // Act
        var result = await Fx.EvaluateAsync("SUM(5, @addValue)", results);

        // Assert
        Assert.Equal(15.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateSumWithArray()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "numbers", new object[] { 1, 2, 3, 4, 5 } }
        };

        // Act
        var result = await Fx.EvaluateAsync("SUM(@numbers)", results);

        // Assert
        Assert.Equal(15.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateSumWithMultipleParameters()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("SUM(1, 2, 3, 4, 5, 6)", results);

        // Assert
        Assert.Equal(21.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateComplexArithmetic()
    {
        // Arrange
        var results = new Dictionary<string, object?>()
        {
            { "valueA", 10 },
            { "valueB", 5 }
        };

        // Act
        var result = await Fx.EvaluateAsync("(@valueA + 2) * 3 - @valueB / 2", results);

        // Assert
        Assert.Equal(((10 + 2) * 3 - 5 / 2.0), Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateArithmeticWithArraySum()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "numbers", new object[] { 1, 2, 3 } }
        };

        // Act
        var result = await Fx.EvaluateAsync("(SUM(@numbers) + 2) * 3", results);

        // Assert
        Assert.Equal(((1 + 2 + 3 + 2) * 3), Convert.ToDouble(result));
    }
}