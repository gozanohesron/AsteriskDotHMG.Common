namespace AsteriskDotHMG.Common.Helpers;

public class ApplicationException : Exception
{
    public ApplicationException() : base()
    {

    }

    public ApplicationException(string message) : base(message)
    {

    }

    public ApplicationException(string message,
        Exception exp) : base(message, exp)
    {

    }

    public ApplicationException(string title,
        string message)
        : base(message) =>
        Title = title;

    public string Title { get; }
}