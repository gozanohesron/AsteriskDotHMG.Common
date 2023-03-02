namespace AsteriskDotHMG.Common.Models;

public class ValidationData
{
    public string Title { get; set; }

    public int StatusCode { get; set; }

    public string Detail { get; set; }

    public List<ValidationError> Errors { get; set; } = new();
}

public class ValidationError
{
    [SwaggerRequired]
    [SwaggerSchema("Field that contains the error")]
    public string Field { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("List of field errors")]
    public List<string> Errors { get; set; } = new();
}

