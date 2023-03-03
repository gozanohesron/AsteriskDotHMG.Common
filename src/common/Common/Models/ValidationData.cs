namespace AsteriskDotHMG.Common.Models;

public class ValidationData
{
    public ValidationData()
    {

    }

    public ValidationData(string title, int statusCode, string detail, List<ValidationError> errors)
    {
        Title = title;
        StatusCode = statusCode;
        Detail = detail;
        Errors = errors;
    }

    public ValidationData(string title, int statusCode, string detail)
    {
        Title = title;
        StatusCode = statusCode;
        Detail = detail;
        Errors = new();
    }

    public string Title { get; set; }

    public int StatusCode { get; set; }

    public string Detail { get; set; }

    public List<ValidationError> Errors { get; set; } = new();
}

public class ValidationError
{
    public ValidationError()
    {

    }

    public ValidationError(string field, List<string> errors)
    {
        Field = field;
        Errors = errors;
    }

    [SwaggerSchema("Field that contains the error")]
    public string Field { get; set; }

    [SwaggerSchema("List of field errors")]
    public List<string> Errors { get; set; } = new();
}

public class ValidationResult
{
    public ValidationResult()
    {

    }

    public ValidationResult(List<ValidationError> errors, string message, string correlationId)
    {
        Errors = errors;
        Message = message;
        CorrelationId = correlationId;
    }

    [SwaggerSchema("Validation message")]
    public string Message { get; set; }

    [SwaggerSchema("List of field validations")]
    public List<ValidationError> Errors { get; set; } = new();

    [SwaggerSchema("Correlation id for troubleshooting")]
    public string CorrelationId { get; set; }
}