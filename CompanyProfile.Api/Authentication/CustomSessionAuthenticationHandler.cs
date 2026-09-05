using System.Security.Claims;
using System.Text.Encodings.Web;
using CompanyProfile.Api.Services;
using CompanyProfile.Core.Entities;
using CompanyProfile.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CompanyProfile.Api.Authentication;

public class CustomSessionAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AppDbContext _db;
    private readonly ISessionTokenService _sessionTokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SessionSettings _sessionSettings;

    public CustomSessionAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        IOptions<AppSettings> appSettings,
        ILoggerFactory logger,
        UrlEncoder encoder,
        AppDbContext db,
        ISessionTokenService sessionTokenService,
        UserManager<ApplicationUser> userManager)
        : base(options, logger, encoder)
    {
        _db = db;
        _sessionTokenService = sessionTokenService;
        _userManager = userManager;
        _sessionSettings = appSettings.Value.Session;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Cookies.TryGetValue(_sessionSettings.CookieName, out var sessionToken) ||
            string.IsNullOrWhiteSpace(sessionToken))
        {
            return AuthenticateResult.NoResult();
        }

        var sessionHash = _sessionTokenService.Hash(sessionToken);
        var now = DateTime.UtcNow;

        var session = await _db.UserSessions
            .FirstOrDefaultAsync(x =>
                x.SessionHash == sessionHash &&
                !x.IsRevoked &&
                x.ExpiresAtUtc > now &&
                x.AbsoluteExpiresAtUtc > now);

        if (session is null)
        {
            return AuthenticateResult.Fail("Invalid or expired session.");
        }

        var currentUserAgent = Request.Headers.UserAgent.ToString();
        var currentUserAgentHash = _sessionTokenService.Hash(currentUserAgent);

        if (!string.Equals(session.UserAgentHash, currentUserAgentHash, StringComparison.Ordinal))
        {
            session.IsRevoked = true;
            await _db.SaveChangesAsync();

            return AuthenticateResult.Fail("Session is no longer valid.");
        }

        var timeRemaining = session.ExpiresAtUtc - now;

        if (timeRemaining <= _sessionSettings.RenewalThresholdDays)
        {
            await RenewSessionAsync(session, now);
        }

        var user = await _userManager.FindByIdAsync(session.UserId);

        if (user is null)
        {
            session.IsRevoked = true;
            await _db.SaveChangesAsync();

            return AuthenticateResult.Fail("Invalid session.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private async Task RenewSessionAsync(UserSession session, DateTime now)
    {
        var newExpiration = now.Add(_sessionSettings.SessionLifetimeDays);

        if (newExpiration > session.AbsoluteExpiresAtUtc)
        {
            newExpiration = session.AbsoluteExpiresAtUtc;
        }

        if (newExpiration <= session.ExpiresAtUtc)
        {
            session.LastAccessedAtUtc = now;
            await _db.SaveChangesAsync();
            return;
        }

        session.LastAccessedAtUtc = now;
        session.ExpiresAtUtc = newExpiration;

        await _db.SaveChangesAsync();
    }
}