namespace AsteriskDotHMG.Common.Models;

public class OperationResult
{
    public OperationResult()
    {

    }

    public OperationResult(bool isSuccess, string message, object data)
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message;
    }

    public OperationResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public OperationResult(bool isSuccess, object data)
    {
        IsSuccess = isSuccess;
        Data = data;
    }

    public OperationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    private string _message = string.Empty;

    [SwaggerSchema("Extra data as further details")]
    public object Data { get; set; }

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
    public BulkOperationResult()
    {

    }

    public BulkOperationResult(int recordsInserted, int recordsUpdated, string message, object data)
    {
        RecordsInserted = recordsInserted;
        RecordsUpdated = recordsUpdated;
        Message = message;
        Data = data;
    }

    public BulkOperationResult(int recordsInserted, int recordsUpdated)
    {
        RecordsInserted = recordsInserted;
        RecordsUpdated = recordsUpdated;
    }

    public BulkOperationResult(int recordsInserted, int recordsUpdated, string message)
    {
        RecordsInserted = recordsInserted;
        RecordsUpdated = recordsUpdated;
        Message = message;
    }

    public BulkOperationResult(int recordsInserted, int recordsUpdated, object data)
    {
        RecordsInserted = recordsInserted;
        RecordsUpdated = recordsUpdated;
        Data = data;
    }

    private string _message = string.Empty;

    [SwaggerSchema("Number of records inserted")]
    public int RecordsInserted { get; set; }

    [SwaggerSchema("Number of records updated")]
    public int RecordsUpdated { get; set; }

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
