using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionX.Tests;
/// <summary>
/// FxComplexTests 클래스는 Fx 클래스의 복잡한 기능을 테스트합니다.
/// </summary>
public class FxComplexTests
{
    [Fact]
    public void TestEvaluateMinWithMax()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "values", new object[] {10, 20, 30, 40, 50} }
            };

        // Act
        var result = Fx.Evaluate("MIN(MAX(values, 35), 60)", results);

        // Assert
        // 입력 값 중에서 최대값이 35보다 크면 35가 되고, 그렇지 않으면 그대로 유지되어야 합니다.
        Assert.Equal(50.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateComplexExpression()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "value1", 10 },
                { "value2", 20 },
                { "value3", 30 }
            };

        // Act
        var result = Fx.Evaluate("(value1 + value2) * value3 / 2", results);

        // Assert
        // 복잡한 수식이 올바르게 계산되어야 합니다.
        Assert.Equal(450.0, Convert.ToDouble(result));
    }

    [Fact]
    public void TestEvaluateMinWithMixedExpressions()
    {
        // Arrange
        var results = new Dictionary<string, object?>
            {
                { "value1", 10 },
                { "value2", 20 },
                { "values", new object[] {5, 15, 25, 35, 45} }
            };

        // Act
        var result = Fx.Evaluate("MIN(value1 + value2, MAX(values))", results);

        // Assert
        // 입력 값 중에서 최소값이 올바르게 계산되어야 합니다.
        Assert.Equal(30.0, Convert.ToDouble(result));
    }
}