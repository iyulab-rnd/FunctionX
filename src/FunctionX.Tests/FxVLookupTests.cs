namespace FunctionX.Tests;

public class FxVLookupTests
{
    [Fact]
    public async Task TestVLookupExactMatchFoundAsync()
    {
        // Arrange
        var range = new List<Dictionary<string, object?>>()
        {
            new() { {"ID", 1}, {"Name", "John Doe"}, {"Age", 30} },
            new() { {"ID", 2}, {"Name", "Jane Doe"}, {"Age", 25} }
        };
        var parameters = new Dictionary<string, object?>()
        {
            { "range", range },
            { "key", 2 },
            { "columnIndex", 3 }, // Age 열의 인덱스
            { "exactMatch", true }
        };

        // Act
        var result = await Fx.EvaluateAsync("VLOOKUP(@key, @range, @columnIndex, @exactMatch)", parameters);

        // Assert
        Assert.Equal(25, result);
    }

    [Fact]
    public async Task TestVLookupExactMatchNotFoundAsync()
    {
        // Arrange
        var range = new List<Dictionary<string, object?>>()
        {
            new() { {"ID", 1}, {"Name", "John Doe"}, {"Age", 30} },
            new() { {"ID", 2}, {"Name", "Jane Doe"}, {"Age", 25} }
        };
        var parameters = new Dictionary<string, object?>()
        {
            { "range", range },
            { "key", 3 }, // 없는 키
            { "columnIndex", 3 }, // Age 열의 인덱스
            { "exactMatch", true }
        };

        // Act
        var result = await Fx.EvaluateAsync("VLOOKUP(@key, @range, @columnIndex, @exactMatch)", parameters);

        // Assert
        Assert.Null(result); // 일치하는 항목이 없으면 null을 반환해야 합니다.
    }

    [Fact]
    public async Task TestVLookupWithApproximateMatchAsync()
    {
        // Arrange
        var range = new object[]
        {
            new Dictionary<string, object?> { {"Score", 50}, {"Grade", "F"} },
            new Dictionary<string, object?> { {"Score", 60}, {"Grade", "D"} },
            new Dictionary<string, object?> { {"Score", 70}, {"Grade", "C"} },
            new Dictionary<string, object?> { {"Score", 80}, {"Grade", "B"} },
            new Dictionary<string, object?> { {"Score", 90}, {"Grade", "A"} }
        };

        var parameters = new Dictionary<string, object?>()
        {
            { "range", range },
            { "key", 85 }, // 근사치 검색 값
            { "columnIndex", 2 }, // Grade 열의 인덱스
            { "exactMatch", false }
        };

        // Act
        var result = await Fx.EvaluateAsync("VLOOKUP(@key, @range, @columnIndex, @exactMatch)", parameters);

        // Assert
        Assert.Equal("B", result); // 85점은 B등급에 해당합니다.
    }

    [Fact]
    public async Task TestVLookupWithStringKeyExactMatchAsync()
    {
        // Arrange
        var range = new List<Dictionary<string, object?>>()
        {
            new() { {"Name", "Alice"}, {"Department", "HR"}, {"Age", 29} },
            new() { {"Name", "Bob"}, {"Department", "IT"}, {"Age", 32} }
        };
        var parameters = new Dictionary<string, object?>()
        {
            { "range", range },
            { "key", "Bob" }, // 문자열 키
            { "columnIndex", 3 }, // Age 열의 인덱스
            { "exactMatch", true }
        };

        // Act
        var result = await Fx.EvaluateAsync("VLOOKUP(@key, @range, @columnIndex, @exactMatch)", parameters);

        // Assert
        Assert.Equal(32, result); // Bob의 나이는 32입니다.
    }

    [Fact]
    public async Task TestVLookupColumnIndexOutOfRangeAsync()
    {
        // Arrange
        var range = new List<Dictionary<string, object?>>()
        {
            new() { {"ID", 1}, {"Name", "John Doe"} },
            new() { {"ID", 2}, {"Name", "Jane Doe"} }
        };
        var parameters = new Dictionary<string, object?>()
        {
            { "range", range },
            { "key", 2 },
            { "columnIndex", 5 }, // 범위를 벗어난 열 인덱스
            { "exactMatch", true }
        };

        // Act
        var result = await Fx.EvaluateAsync("VLOOKUP(@key, @range, @columnIndex, @exactMatch)", parameters);

        // Assert
        Assert.Null(result); // 범위를 벗어난 열 인덱스는 null을 반환해야 합니다.
    }
}