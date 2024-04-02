namespace FunctionX.Tests;

public static class MyFunctions
{
    public static double MyFunc(double x, double y)
    {
        return (x + y) * 2;
    }
}

public class FxCustomFuncTests
{
    [Fact]
    public async Task TestXorFunction_WithOneTrueOneFalse_ReturnsTrue()
    {
        // Act
        var result = await Fx.EvaluateAsync(
            expression: "MyFunctions.MyFunc(1, 2)", 
            customFuncType: typeof(MyFunctions));

        // Assert
        Assert.Equal((1.0 + 2.0) * 2.0, result);
    }
}