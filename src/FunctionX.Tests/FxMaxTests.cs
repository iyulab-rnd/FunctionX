namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxMaxTests
{
    [Fact]
    public async Task TestEvaluateMaxWithConstants()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("MAX(10, 20, 5, 15)", results);

        // Assert
        Assert.Equal(20.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateMaxWithVariable()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "maxValues", new object[] {10, 20, 5, 15} }
            };

        // Act
        var result = await Fx.EvaluateAsync("MAX(@maxValues)", results);

        // Assert
        Assert.Equal(20.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateMaxWithEmptyArray()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "emptyArray", Array.Empty<object>() }
            };

        // Act
        var result = await Fx.EvaluateAsync("MAX(@emptyArray)", results);

        // Assert
        Assert.Equal(double.NaN, Convert.ToDouble(result));
    }
}