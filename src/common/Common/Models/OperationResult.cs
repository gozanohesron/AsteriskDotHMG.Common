namespace AsteriskDotHMG.Common.Models;

public class OperationResult
{
    private string _message = string.Empty;

    [SwaggerRequired]
    [SwaggerSchema("Extra data as further details")]
    public object Data { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("Determine whether the operation is successful or not")]
    public bool IsSuccess { get; set; }

    [SwaggerSchema("Operation message")]
    public string Message
    {
        get
        {
            if (string.IsNullOrEmpty(_message))
            {
                return IsSuccess ? "Operation successful" : "Operation failed";
            }

            return _message;

        }
        set { _message = value; }
    }
}

public class BulkOperationResult
{

    private string _message = string.Empty;

    [SwaggerRequired]
    [SwaggerSchema("Number of records inserted")]
    public int RecordsInserted { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("Number of records updated")]
    public int RecordsUpdated { get; set; }

    [SwaggerRequired]
    [SwaggerSchema("Extra data as further details")]
    public object Data { get; set; }

    [SwaggerSchema("Operation message")]
    public string Message
    {
        get
        {
            if (string.IsNullOrEmpty(_message))
            {
                return StaticMethods.CreateOperationMessage(RecordsInserted, RecordsUpdated);
            }

            return _message;

        }
        set { _message = value; }
    }

    [SwaggerSchema("Determine whether the operation is successful or not")]
    public bool IsSuccess => RecordsInserted > 0 || RecordsUpdated > 0;
}
