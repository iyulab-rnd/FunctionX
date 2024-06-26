namespace FunctionX.Tests;

/// <summary>
/// FxTests 클래스는 Fx 클래스의 메서드를 테스트합니다.
/// 엑셀 함수의 사용을 모방하여 다양한 테스트를 구현합니다.
/// </summary>
public class FxIndexTests
{

    [Fact]
    public async Task TestIndexFunction()
    {
        // Arrange1
        var range = new object[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }, new object[] { 7, 8, 9 } };
        var parameters = new Dictionary<string, object?> { { "range", range } };

        // Act
        var result = await Fx.EvaluateAsync("INDEX(@range, 1, 1)", parameters);

        // Assert
        Assert.Equal(1, Convert.ToDouble(result));
    }

    [Fact]
    public async Task TestIndexOutOfRange()
    {
        // Arrange
        var range = new object[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }, new object[] { 7, 8, 9 } };
        var parameters = new Dictionary<string, object?> { { "range", range } };

        await Assert.ThrowsAsync<FxReferenceException>(async () =>
        {
            var result = await Fx.EvaluateAsync("INDEX(@range, 5, 5)", parameters); // 범위를 벗어난 인덱스 조회
        });
    }

    [Fact]
    public async Task TestConcatFunction_WithIndexResult()
    {
        // Arrange
        var range = new List<Dictionary<string, object?>>
        {
            new() { { "Name", "John" }, { "Age", 30 } },
            new() { { "Name", "Alice" }, { "Age", 25 } }
        };
        var parameters = new Dictionary<string, object?>
        {
            { "range", range },
            { "rowIndex", 1 },
            { "columnIndex", 0 }
        };

        // Act
        var result = await Fx.EvaluateAsync("INDEX(@range, @rowIndex, @columnIndex + 1)", parameters);

        // Assert
        Assert.Equal("John", result);
    }

    [Fact]
    public async Task TestIndexByListDictionary()
    {
        // Arrange
        var listDictionary = new List<Dictionary<string, object?>>
        {
            new() { { "A", 1 }, { "B", 2 }, { "C", 3 } },
            new() { { "A", 4 }, { "B", 5 }, { "C", 6 } },
            new() { { "A", 7 }, { "B", 8 }, { "C", 9 } }
        };
        var parameters = new Dictionary<string, object?> { { "range", listDictionary } };

        // Act
        var result = await Fx.EvaluateAsync("INDEX(@range, 2, \"B\")", parameters);

        // Assert
        Assert.Equal(5, Convert.ToDouble(result)); // 범위의 해당 위치에 있는 값은 5여야 합니다.
    }
}