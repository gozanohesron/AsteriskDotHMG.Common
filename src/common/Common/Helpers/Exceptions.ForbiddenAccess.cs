namespace AsteriskDotHMG.Common.Helpers;

public class ForbiddenAccessException : ApplicationException
{
    public ForbiddenAccessException(string message)
       : base("Forbidden Access", message)
    {
    }

    public ForbiddenAccessException() : base()
    {

    }

    public ForbiddenAccessException(string message,
        Exception exp) : base(message, exp)
    {

    }
}