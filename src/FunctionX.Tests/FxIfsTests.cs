namespace FunctionX.Tests;

public class FxIfsTests
{
    [Fact]
    public async Task TestIfsFunction_FirstConditionTrue_ReturnsFirstResult()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("IFS(true, \"First\", false, \"Second\")", parameters);

        // Assert
        Assert.Equal("First", result);
    }

    [Fact]
    public async Task TestIfsFunction_SecondConditionTrue_ReturnsSecondResult()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("IFS(false, \"First\", true, \"Second\")", parameters);

        // Assert
        Assert.Equal("Second", result);
    }

    [Fact]
    public async Task TestIfsFunction_NoTrueCondition_ReturnsNull()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>();

        // Act
        var result = await Fx.EvaluateAsync("IFS(false, \"First\", false, \"Second\")", parameters);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task TestIfsFunction_WithVariables()
    {
        // Arrange
        var parameters = new Dictionary<string, object?>
            {
                { "condition1", false },
                { "condition2", true },
                { "condition3", false }
            };

        // Act
        var result = await Fx.EvaluateAsync("IFS(@condition1, \"First\", @condition2, \"Second\", @condition3, \"Third\")", parameters);

        // Assert
        Assert.Equal("Second", result);
    }
}
