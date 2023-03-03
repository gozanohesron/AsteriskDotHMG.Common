namespace AsteriskDotHMG.Common.Models;

public class ErrorInfo<TModel>
{
    public ErrorInfo()
    {

    }

    public ErrorInfo(string message, string correlationId, TModel data)
    {
        Message = message;
        CorrelationId = correlationId;
        Data = data;
    }

    public ErrorInfo(string message, string correlationId)
    {
        Message = message;
        CorrelationId = correlationId;
    }

    [SwaggerSchema("Extra data as further details")]
    public TModel Data { get; set; }

    [SwaggerSchema("Error message")]
    public string Message { get; set; }

    [SwaggerSchema("Correlation id for troubleshooting")]
    public string CorrelationId { get; set; }
}

public class TriggerError<TModel>
{
    public TriggerError()
    {

    }

    public TriggerError(string message, string correlationId, TModel data, List<ValidationError> errors)
    {
        Message = message;
        CorrelationId = correlationId;
        Data = data;
        Errors = errors;
    }

    public TriggerError(string message, string correlationId, TModel data)
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

    public TModel Data { get; set; }

    public string Message { get; set; }

    public string CorrelationId { get; set; }

    public List<ValidationError> Errors { get; set; } = new();
}