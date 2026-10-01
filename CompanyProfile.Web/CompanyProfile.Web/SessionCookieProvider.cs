using Microsoft.AspNetCore.Http;

namespace CompanyProfile.Web.Authentication;

public sealed class SessionCookieProvider(IHttpContextAccessor httpContextAccessor)
{
    public Task<string?> GetCookieAsync(CancellationToken cancellationToken)
    {
        var cookie = httpContextAccessor.HttpContext?.Request.Cookies["__Host-user_session"];
        return Task.FromResult(cookie);
    }
}