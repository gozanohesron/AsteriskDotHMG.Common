namespace AsteriskDotHMG.Common.Helpers;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string message)
       : base("Bad Request", message)
    {
    }

    public BadRequestException() : base()
    {

    }

    public BadRequestException(string message,
        Exception exp) : base(message, exp)
    {

    }
}