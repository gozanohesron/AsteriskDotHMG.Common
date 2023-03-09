namespace AsteriskDotHMG.Common.Helpers;

public static class Enums
{
    public enum Action
    {
        Create,
        Update,
        Delete
    }

    public enum ProcessResult
    {
        Success,
        Failure
    }

    public enum FilterCondition
    {
        Or,
        And
    }

    public enum FilterOperator
    {
        Equal,
        NotEqual,
        Contains,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        EndsWith,
        StartsWith
    }
}
