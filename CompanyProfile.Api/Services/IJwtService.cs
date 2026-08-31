namespace CompanyProfile.Api.Services;

public interface IJwtService
{
    Task<(string AccessToken, DateTime ExpiresAtUtc)> GenerateTokenAsync(
        string userId,
        string userName,
        string? email,
        IEnumerable<string> roles);
}