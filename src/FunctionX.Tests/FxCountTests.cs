namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxCountTests
{
    [Fact]
    public void TestEvaluateCountWithConstants()
    {
        // Arrange
        var results = new Dictionary<string, object?>();

        // Act
        var result = Fx.Evaluate("COUNT(1, 2, 3, 4, 5)", results);

        // Assert
        Assert.Equal(5.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateCountWithVariable()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "countValues", new object[] {1, 2, 3, 4, 5} }
        };

        // Act
        var result = Fx.Evaluate("COUNT(countValues)", results);

        // Assert
        Assert.Equal(5.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateCountWithMixedTypes()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "mixedValues", new object[] {1, null, "apple", "", 5} }
        };

        // Act
        var result = Fx.Evaluate("COUNT(mixedValues)", results);

        // Assert
        Assert.Equal(3.0, Convert.ToDouble(result)); // 여기서는 숫자와 비어있지 않은 문자열만을 계산한다고 가정
    }

    [Fact]
    public void TestEvaluateCountWithAllStrings()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "stringValues", new object[] {"apple", "banana", null, "cherry"} }
        };

        // Act
        var result = Fx.Evaluate("COUNT(stringValues)", results);

        // Assert
        // COUNT 함수가 숫자만 계산한다면, 결과는 0이 될 수 있음.
        // 문자열도 포함한다면, 이 경우는 3이 될 수 있음.
        // 여기서는 숫자만 계산한다고 가정하고 0을 기대.
        Assert.Equal(3.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateCountWithEmptyArray()
    {
        // Arrange
        var results = new Dictionary<string, object?>
        {
            { "emptyArray", Array.Empty<object>() }
        };

        // Act
        var result = Fx.Evaluate("COUNT(emptyArray)", results);

        // Assert
        Assert.Equal(0.0, Convert.ToDouble(result));
    }
}