namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxMaxTests
{
    [Fact]
    public void TestEvaluateMaxWithConstants()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = Fx.Evaluate("MAX(10, 20, 5, 15)", results);

        // Assert
        Assert.Equal(20.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateMaxWithVariable()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "maxValues", new object[] {10, 20, 5, 15} }
            };

        // Act
        var result = Fx.Evaluate("MAX(maxValues)", results);

        // Assert
        Assert.Equal(20.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateMaxWithMixedTypes()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "mixedValues", new object[] {2, "10", 30, "six"} }
            };

        // Act
        var result = Fx.Evaluate("MAX(mixedValues)", results);

        // Assert
        Assert.Equal(30.0, Convert.ToDouble(result)); // 여기서는 숫자만 고려하고 문자열은 무시한다고 가정
    }

    [Fact]
    public void TestEvaluateMaxWithEmptyArray()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "emptyArray", Array.Empty<object>() }
            };

        // Act
        var result = Fx.Evaluate("MAX(emptyArray)", results);

        // Assert
        Assert.Null(result);
    }
}