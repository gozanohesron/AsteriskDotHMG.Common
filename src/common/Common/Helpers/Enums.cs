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

    public enum QueryFilterCondition
    {
        Or,
        And
    }

    public enum QueryFilterOperator
    {
        StartsWith,
        EndsWith,
        Equal,
        NotEqual,
        Contains
    }
}
