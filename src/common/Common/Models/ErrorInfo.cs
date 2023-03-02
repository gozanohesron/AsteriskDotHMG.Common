namespace AsteriskDotHMG.Common.Models;

public class ErrorInfo
{
    [SwaggerSchema("Extra data as further details")]
    public object Data { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("Error message")]
    public string Message { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("Correlation id for troubleshooting")]
    public string CorrelationId { get; set; }
}

public class ValidationResult
{
    [SwaggerRequired]
    [SwaggerSchema("Validation message")]
    public string Message { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("List of field validations")]
    public List<ValidationError> Errors { get; set; } = new();

    [SwaggerRequired]
    [SwaggerSchema("Correlation id for troubleshooting")]
    public string CorrelationId { get; set; }
}
