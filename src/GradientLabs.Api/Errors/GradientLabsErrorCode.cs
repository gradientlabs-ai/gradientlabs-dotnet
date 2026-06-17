namespace GradientLabs.Api;

public readonly record struct GradientLabsErrorCode(string Value)
{
    public static readonly GradientLabsErrorCode NotFound = new("not_found");
    public static readonly GradientLabsErrorCode Unauthenticated = new("unauthenticated");
    public static readonly GradientLabsErrorCode PermissionDenied = new("permission_denied");
    public static readonly GradientLabsErrorCode InvalidArgument = new("invalid_argument");
    public static readonly GradientLabsErrorCode FailedPrecondition = new("failed_precondition");
    public static readonly GradientLabsErrorCode ResourceExhausted = new("resource_exhausted");
    public static readonly GradientLabsErrorCode AlreadyExists = new("already_exists");
    public static readonly GradientLabsErrorCode Unavailable = new("unavailable");
    public static readonly GradientLabsErrorCode DeadlineExceeded = new("deadline_exceeded");
    public static readonly GradientLabsErrorCode Internal = new("internal");
    public static readonly GradientLabsErrorCode Aborted = new("aborted");
    public static readonly GradientLabsErrorCode Unknown = new("unknown");

    public bool IsRetryable =>
        this == Unavailable ||
        this == DeadlineExceeded ||
        this == Aborted ||
        this == Unknown ||
        this == Internal;

    public override string ToString() => Value;
    public static implicit operator string(GradientLabsErrorCode v) => v.Value;
}
