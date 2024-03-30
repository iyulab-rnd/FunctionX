namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxAverageTests
{
    [Fact]
    public async Task TestEvaluateAverageWithConstants()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("AVERAGE(2, 4, 6, 8)", results);

        // Assert
        Assert.Equal(5.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateAverageWithVariable()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "avgValue", new object[] {2, 4, 6, 8} }
        };

        // Act
        var result = await Fx.EvaluateAsync("AVERAGE(@avgValue)", results);

        // Assert
        Assert.Equal(5.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateAverageWithEmptyArray()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "emptyArray", Array.Empty<object>() }
        };

        // Act
        var result = await Fx.EvaluateAsync("AVERAGE(@emptyArray)", results);

        // Assert
        Assert.Equal(double.NaN, Convert.ToDouble(result));
    }
}