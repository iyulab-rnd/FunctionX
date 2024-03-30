namespace FunctionX.Tests;

public class FxSwitchTests
{
    [Fact]
    public async Task TestSwitchFunction_MatchFirstCase_ReturnsFirstResult()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("SWITCH(\"A\", \"A\", \"First\", \"B\", \"Second\")", parameters);

        // Assert
        Assert.Equal("First", result);
    }

    [Fact]
    public async Task TestSwitchFunction_MatchSecondCase_ReturnsSecondResult()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("SWITCH(\"B\", \"A\", \"First\", \"B\", \"Second\")", parameters);

        // Assert
        Assert.Equal("Second", result);
    }

    [Fact]
    public async Task TestSwitchFunction_NoMatch_ReturnsNull()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("SWITCH(\"C\", \"A\", \"First\", \"B\", \"Second\")", parameters);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task TestSwitchFunction_WithVariable()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
            {
                { "var", "B" }
            };

        // Act
        var result = await Fx.EvaluateAsync("SWITCH(@var, \"A\", \"First\", \"B\", \"Second\")", parameters);

        // Assert
        Assert.Equal("Second", result);
    }
}
