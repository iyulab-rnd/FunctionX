namespace FunctionX.Tests;
/// <summary>
/// FxComplexTests 클래스는 Fx 클래스의 복합적인 사용성을 테스트합니다.
/// </summary>
public class FxComplexTests
{
    [Fact]
    public async Task TestEvaluateMinWithMax()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "values", new object[] {10, 20, 30, 40, 50} }
            };

        // Act
        var result = await Fx.EvaluateAsync("MIN(MAX(@values, 35), 60)", results);

        // Assert
        // 입력 값 중에서 최대값이 35보다 크면 35가 되고, 그렇지 않으면 그대로 유지되어야 합니다.
        Assert.Equal(50.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateComplexExpression()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "value1", 10 },
                { "value2", 20 },
                { "value3", 30 }
            };

        // Act
        var result = await Fx.EvaluateAsync("(@value1 + @value2) * @value3 / 2", results);

        // Assert
        // 복잡한 수식이 올바르게 계산되어야 합니다.
        Assert.Equal(450.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateMinWithMixedExpressions()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "value1", 10 },
                { "value2", 20 },
                { "values", new object[] {5, 15, 25, 35, 45} }
            };

        // Act
        var result = await Fx.EvaluateAsync("MIN(@value1 + @value2, MAX(@values))", results);

        // Assert
        // 입력 값 중에서 최소값이 올바르게 계산되어야 합니다.
        Assert.Equal(30.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateCountUnique()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
        {
            { "values", new object[] { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 } }
        };

        // Act
        // COUNT(UNIQUE(@values)) - values 배열의 고유 값 개수
        var result = await Fx.EvaluateAsync("COUNT(UNIQUE(@values))", parameters);

        // Assert
        // 고유 값의 개수를 계산해야 합니다. 즉, 1, 2, 3, 4로 총 4가 되어야 합니다.
        Assert.Equal(4.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateComplexConditional()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
        {
            { "value1", 100 },
            { "value2", 200 },
            { "condition", true }
        };

        // Act
        // IF(@condition, @value1 + @value2, @value1 - @value2)
        var result = await Fx.EvaluateAsync("IF(@condition, @value1 + @value2, @value1 - @value2)", parameters);

        // Assert
        // condition이 참이므로, value1과 value2의 합을 반환해야 합니다. 즉, 100 + 200 = 300이 되어야 합니다.
        Assert.Equal(300.0, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestEvaluateSwitchFunction()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
        {
            { "day", 3 } // 예를 들어, 3은 "Wednesday"를 의미
        };

        // Act
        var result = await Fx.EvaluateAsync("SWITCH(@day, 1, \"Monday\", 2, \"Tuesday\", 3, \"Wednesday\", 4, \"Thursday\", 5, \"Friday\", \"Unknown\")", parameters);

        // Assert
        // day가 3이므로, "Wednesday"를 반환해야 합니다.
        Assert.Equal("Wednesday", result);
    }
}