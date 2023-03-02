namespace AsteriskDotHMG.Common.Methods;

public static partial class StaticMethods
{
    public static string CreateOperationMessage(int recordsInserted, int recordsUpdated)
    {
        string result = "No records affected";

        if (recordsInserted > 0 || recordsUpdated > 0)
        {
            if (recordsInserted > 0 && recordsUpdated > 0)
            {
                result = $"{recordsInserted} {(recordsInserted > 1 ? "rows" : "row")} added and {recordsUpdated} {(recordsUpdated > 1 ? "rows" : "row")} updated";
            }
            else if (recordsInserted > 0 && recordsUpdated == 0)
            {
                result = $"{recordsInserted} {(recordsInserted > 1 ? "rows" : "row")} added";
            }
            else if (recordsInserted == 0 && recordsUpdated > 0)
            {
                result = $"{recordsUpdated} {(recordsUpdated > 1 ? "rows" : "row")} updated";
            }
        }

        return result;
    }
}
