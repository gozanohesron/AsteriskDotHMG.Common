namespace AsteriskDotHMG.Common.Models;

public class ErrorInfo
{
    public ErrorInfo()
    {

    }

    public ErrorInfo(string message, string correlationId, object data = null)
    {
        Message = message;
        CorrelationId = correlationId;
        Data = data;
    }

    [SwaggerSchema("Extra data as further details")]
    public object Data { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("Error message")]
    public string Message { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("Correlation id for troubleshooting")]
    public string CorrelationId { get; set; }
}

public class TriggerError
{
    public TriggerError()
    {

    }

    public TriggerError(string message, string correlationId, object data, List<ValidationError> errors)
    {
        Message = message;
        CorrelationId = correlationId;
        Data = data;
        Errors = errors;
    }

    public TriggerError(string message, string correlationId, object data)
    {
        Message = message;
        CorrelationId = correlationId;
        Data = data;
        Errors = new();
    }

    public TriggerError(string message, string correlationId)
    {
        Message = message;
        CorrelationId = correlationId;
        Errors = new();
    }

    public TriggerError(string message, string correlationId, List<ValidationError> errors)
    {
        Message = message;
        CorrelationId = correlationId;
        Errors = errors;
    }

    public object Data { get; set; }

    public string Message { get; set; }

    public string CorrelationId { get; set; }

    public List<ValidationError> Errors { get; set; } = new();
}