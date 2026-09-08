namespace CompanyProfile.Shared.DTOs.Auth
{
    public sealed record LoginRequest(string UserName,string Password);

    public sealed record LoginResponse(string AccessToken,DateTime ExpiresAtUtc);
}
