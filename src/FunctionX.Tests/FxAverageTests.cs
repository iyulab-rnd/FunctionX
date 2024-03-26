namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxAverageTests
{
    [Fact]
    public void TestEvaluateAverageWithConstants()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = Fx.Evaluate("AVERAGE(2, 4, 6, 8)", results);

        // Assert
        Assert.Equal(5.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateAverageWithVariable()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "avgValue", new object[] {2, 4, 6, 8} }
        };

        // Act
        var result = Fx.Evaluate("AVERAGE(avgValue)", results);

        // Assert
        Assert.Equal(5.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateAverageWithEmptyArray()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "emptyArray", Array.Empty<object>() }
        };

        // Act
        var result = Fx.Evaluate("AVERAGE(emptyArray)", results);

        // Assert
        Assert.Equal(0.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateAverageWithMixedTypes()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "mixedValues", new object[] {1, "two", 3, "four"} }
            };

        // Act
        var result = Fx.Evaluate("AVERAGE(mixedValues)", results);

        // Assert
        Assert.Equal(2.0, Convert.ToDouble(result)); // 여기서는 숫자만 평균을 내고 문자열은 무시한다고 가정
    }
}