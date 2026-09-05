namespace CompanyProfile.Api.Common;

public record ApiResponse<T>(
    bool Success,
    string Message,
    T? Data)
{
    public static ApiResponse<T> SuccessResponse(
        T? data,
        string message = "Success")
        => new(true, message, data);

    public static ApiResponse<T> FailureResponse(
        string message)
        => new(false, message, default);
}