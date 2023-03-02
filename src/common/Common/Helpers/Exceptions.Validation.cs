using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace AsteriskDotHMG.Common.Helpers;

public class ValidationException : ApplicationException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
       : base("Validation Failure", "One or more validation errors occurred")
       => ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }

    public ValidationException()
        : base("Validation Failure", "One or more validation failures have occurred.")
    {
        Dictionary<string, string[]> dictionary = new();
        Errors = dictionary;
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public ValidationException(IEnumerable<IdentityError> errors) : this()
    {
        Errors = errors
            .GroupBy(e => e.Code, e => e.Description)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}