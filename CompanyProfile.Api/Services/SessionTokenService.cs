using System.Security.Cryptography;
using System.Text;

namespace CompanyProfile.Api.Services;

public class SessionTokenService : ISessionTokenService
{
    public string GenerateSessionToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }

    public string Hash(string token)
    {
        var hash = SHA256.HashData(
            Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(hash);
    }
}