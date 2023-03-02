namespace AsteriskDotHMG.Common.Helpers;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string message)
        : base("Not Found", message)
    {
    }

    public NotFoundException() : base()
    {

    }

    public NotFoundException(string message,
        Exception exp) : base(message, exp)
    {

    }

    public NotFoundException(string name,
        object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {

    }
}