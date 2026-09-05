namespace CompanyProfile.Api.Services;

public interface ISessionTokenService
{
    string GenerateSessionToken();

    string Hash(string token);

}