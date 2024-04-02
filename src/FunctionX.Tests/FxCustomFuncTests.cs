namespace FunctionX.Tests;

public static class MyFunctions
{
    public static double MyFunc(object x, object y)
    {
        var x1 = Convert.ToDouble(x);
        var y1 = Convert.ToDouble(y);
        return (x1 + y1) * 2.0;
    }
}

public class FxCustomFuncTests
{
    [Fact]
    public async Task MyFuncTest()
    {
        var x = 1.0;
        var y = 2.0;

        var parameters = new Dictionary<string, object?>
        {
            { "x", x },
            { "y", y }
        };

        // Act
        var result = await Fx.EvaluateAsync(
            expression: "MyFunctions.MyFunc(@x, @y) + 5", 
            parameters: parameters,
            customFuncType: typeof(MyFunctions));

        // Assert
        var r1 = (x + y) * 2.0;
        var r2 = r1 + 5;
        Assert.Equal(r2, result);
    }
}