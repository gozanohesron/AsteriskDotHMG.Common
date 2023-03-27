using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AsteriskDotHMG.Common.Models;

public class OperationResult
{
    public OperationResult()
    {

    }

    public OperationResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public OperationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    private string _message = string.Empty;

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

public class OperationResult<TModel>: OperationResult
{
    public OperationResult()
    {

    }

    public OperationResult(bool isSuccess, string message, TModel data)
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message;
    }

    public OperationResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public OperationResult(bool isSuccess, TModel data)
    {
        IsSuccess = isSuccess;
        Data = data;
    }

    public OperationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    [SwaggerSchema("Extra data as further details")]
    public TModel Data { get; set; }
}


public class BulkOperationResult
{
    public BulkOperationResult()
    {

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

    private string _message = string.Empty;

    [SwaggerSchema("Number of records inserted")]
    public int RecordsInserted { get; set; }

    [SwaggerSchema("Number of records updated")]
    public int RecordsUpdated { get; set; }

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


public class BulkOperationResult<TModel> : BulkOperationResult
{
    public BulkOperationResult()
    {

    }

    public BulkOperationResult(int recordsInserted, int recordsUpdated, string message, TModel data)
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

    public BulkOperationResult(int recordsInserted, int recordsUpdated, TModel data)
    {
        RecordsInserted = recordsInserted;
        RecordsUpdated = recordsUpdated;
        Data = data;
    }

    [SwaggerSchema("Extra data as further details")]
    public TModel Data { get; set; }
}
