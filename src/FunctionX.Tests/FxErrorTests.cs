namespace FunctionX.Tests;

public class FxErrorTests
{
    [Fact]
    public async Task FxIfErrorOk()
    {
        // Act
        var result = await Fx.EvaluateAsync("IFERROR(10 / 5, \"ERROR\")");

        // Assert
        Assert.Equal(10 / 5, result);
    }

    [Fact]
    public async Task FxIfErrorError()
    {
        var parameters = new Dictionary<string, object?>
        {
            { "a", 10.0 },
            { "b", 0.0 }
        };
        // Act
        var result = await Fx.EvaluateAsync("IFERROR(INT(@a) / INT(@b), \"ERROR\")", parameters);

        // Assert
        Assert.Equal("ERROR", result);
    }

    [Fact]
    public async Task FxIfError1()
    {
        var parameters = new Dictionary<string, object?>
        {
            { "a", "hello" }
        };
        // Act
        var result = await Fx.EvaluateAsync("IFERROR(INT(@a) + 1, \"ERROR\")", parameters);

        // Assert
        Assert.Equal("ERROR", result);
    }

    [Fact]
    public async Task FxIfErrorDeep()
    {
        var parameters = new Dictionary<string, object?>
        {
            { "a", 10.0 },
            { "b", 0.0 }
        };
        // Act
        var result = await Fx.EvaluateAsync("IFERROR(10 / INT(IFERROR(INT(@a) / INT(@b), \"ERROR\")), \"ON ERROR\")", parameters);

        // Assert
        Assert.Equal("ON ERROR", result);
    }
}