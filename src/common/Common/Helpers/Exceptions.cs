namespace AsteriskDotHMG.Common.Helpers;

public class ServerException : Exception, ICustomException
{
    public HttpStatusCode StatusCode { get; }
    public object ExceptionData { get; }

    public ServerException(string message,
        HttpStatusCode statuscode = HttpStatusCode.InternalServerError,
        object data = null)
        : base(message)
    {
        StatusCode = statuscode;
        ExceptionData = data;
    }

    public ServerException(string message,
        Exception innerException)
        : base(message, innerException)
    {
    }

    public static readonly ServerException InternalServerError =
        new("Internal Server Error", HttpStatusCode.BadRequest);
}
