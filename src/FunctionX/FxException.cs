namespace FunctionX;

public abstract class FxException(string message) : Exception(message)
{
}

// #DIV/0! 
public class FxDivideByZeroException : FxException
{
    public FxDivideByZeroException(string message = "#DIV/0!") : base(message)
    {
    }

    public override string ToString() => "#DIV/0!";
}

// #VALUE!
public class FxValueException : FxException
{
    public FxValueException(string message = "#VALUE!") : base(message)
    {
    }

    public override string ToString() => "#VALUE!";
}

// #REF!
public class FxReferenceException : FxException
{
    public FxReferenceException(string message = "#REF!") : base(message)
    {
    }

    public override string ToString() => "#REF!";
}

// #NAME?
public class FxNameException : FxException
{
    public FxNameException(string message = "#NAME?") : base(message)
    {
    }

    public override string ToString() => "#NAME?";
}

// #NUM!
public class FxNumException : FxException
{
    public FxNumException(string message = "#NUM!") : base(message)
    {
    }

    public override string ToString() => "#NUM!";
}

// #N/A
public class FxNAException : FxException
{
    public FxNAException(string message = "#N/A") : base(message)
    {
    }

    public override string ToString() => "#N/A";
}

public class FxUnsafeExpressionException : FxException
{
    public FxUnsafeExpressionException(string message = "Unsafe expression") : base(message)
    {
    }
}

public class FxExpressionException : FxException
{
    public FxExpressionException(string message) : base(message)
    {
    }
}

public class FxCompilationErrorException : FxException
{
    public FxCompilationErrorException(string message = $"Compilation Error") : base(message)
    {
    }
}