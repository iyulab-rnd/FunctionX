namespace FunctionX;

public class FxException(string message) : Exception(message)
{
}

// #DIV/0! 
public class FxDivideByZeroException : FxException
{
    public FxDivideByZeroException() : base("#DIV/0!")
    {
    }
}

// #VALUE!
public class FxValueException : FxException
{
    public FxValueException() : base("#VALUE!")
    {
    }
}

// #REF!
public class FxReference : FxException
{
    public FxReference() : base("#REF!")
    {
    }
}

// #NAME?
public class FxNameException : FxException
{
    public FxNameException() : base("#NAME?")
    {
    }
}

// #NUM!
public class FxNumException : FxException
{
    public FxNumException() : base("#NUM!")
    {
    }
}
// #N/A
public class FxNAException : FxException
{
    public FxNAException() : base("#N/A")
    {
    }
}

public class FxUnsafeExpressionException : FxException
{
    public FxUnsafeExpressionException(string keyword) : base($"Unsafe expression: {keyword}")
    {
    }
}