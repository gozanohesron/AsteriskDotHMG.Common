namespace AsteriskDotHMG.Common.Helpers;

public class IdentityException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public new object Data { get; }

    public IdentityException(string message,
        HttpStatusCode statuscode = HttpStatusCode.InternalServerError,
        object data = null)
        : base(message)
    {
        StatusCode = statuscode;
        Data = data;
    }

    public IdentityException(string message,
        Exception innerException)
        : base(message, innerException)
    {
    }

    public static readonly IdentityException InvalidUsernamePassword =
        new("Invalid username or password", HttpStatusCode.BadRequest);
    public static readonly IdentityException UserNotFound =
        new("User not found", HttpStatusCode.BadRequest);
    public static readonly IdentityException RoleNotFound =
        new("Role not found", HttpStatusCode.BadRequest);
    public static readonly IdentityException DeleteAdminFailed =
        new("You can not delete Administrator Role", HttpStatusCode.BadRequest);
    public static readonly IdentityException NoRolesFound =
        new("No valid roles found on the provided role list", HttpStatusCode.BadRequest);
    public static readonly IdentityException InvalidAccessRefreshToken =
        new("Invalid access token or refresh token", HttpStatusCode.BadRequest);
    public static readonly IdentityException DataIdDoesNotMatchParameter =
        new("Data id does not match the parameter id", HttpStatusCode.BadRequest);
}