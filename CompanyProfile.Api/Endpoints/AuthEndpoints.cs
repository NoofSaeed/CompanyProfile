using CompanyProfile.Api.Common;
using CompanyProfile.Api.Services;
using CompanyProfile.Core.DTOs.Auth;
using CompanyProfile.Core.Entities;
using CompanyProfile.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CompanyProfile.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/login", LoginAsync).AllowAnonymous().WithName("Login");
        group.MapPost("/logout", LogoutAsync).RequireAuthorization().WithName("Logout");
        group.MapGet("/sessions", GetActiveSessionsAsync).RequireAuthorization().WithName("GetActiveSessions");
        group.MapDelete("/sessions/{sessionId}", RevokeSessionAsync).RequireAuthorization().WithName("RevokeSession");
        group.MapPost("/sessions/revoke-all", RevokeAllSessionsAsync).RequireAuthorization().WithName("RevokeAllSessions");
        group.MapPost("/sessions/revoke-others", RevokeOtherSessionsAsync).RequireAuthorization().WithName("RevokeOtherSessions");

        return app;
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        UserManager<ApplicationUser> userManager,
        ISessionTokenService sessionTokenService,
        AppDbContext db,
        IOptions<AppSettings> appSettings,
        HttpRequest httpRequest,
        HttpResponse httpResponse)
    {
        var sessionSettings = appSettings.Value.Session;

        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Results.Json(
                ApiResponse<object>.FailureResponse(ApiMessages.InvalidCredentials),
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var now = DateTime.UtcNow;
        var sessionToken = sessionTokenService.GenerateSessionToken();
        var sessionHash = sessionTokenService.Hash(sessionToken);
        var userAgent = GetUserAgent(httpRequest);
        var userAgentHash = sessionTokenService.Hash(userAgent);
        var absoluteExpiresAt = now.AddDays(sessionSettings.AbsoluteSessionLifetimeDays.Days);

        var userSession = new UserSession
        {
            UserId = user.Id,
            SessionHash = sessionHash,
            IpAddress = GetClientIp(httpRequest),
            UserAgent = userAgent,
            UserAgentHash = userAgentHash,
            CreatedAtUtc = now,
            LastAccessedAtUtc = now,
            ExpiresAtUtc = now.AddDays(sessionSettings.SessionLifetimeDays.Days),
            AbsoluteExpiresAtUtc = absoluteExpiresAt,
            IsRevoked = false
        };

        db.UserSessions.Add(userSession);
        await db.SaveChangesAsync();

        SetSessionCookie(
            httpResponse,
            sessionToken,
            TimeSpan.FromDays(sessionSettings.AbsoluteSessionLifetimeDays.Days),
            sessionSettings.CookieName);

        return Results.Ok(ApiResponse<object>.SuccessResponse(null, ApiMessages.Success));
    }

    private static async Task<IResult> LogoutAsync(
        HttpRequest httpRequest,
        HttpResponse httpResponse,
        ISessionTokenService sessionTokenService,
        IOptions<AppSettings> appSettings,
        AppDbContext db)
    {
        var sessionSettings = appSettings.Value.Session;

        if (httpRequest.Cookies.TryGetValue(sessionSettings.CookieName, out var sessionToken))
        {
            var sessionHash = sessionTokenService.Hash(sessionToken);
            var session = await db.UserSessions.FirstOrDefaultAsync(x => x.SessionHash == sessionHash);

            if (session is not null && !session.IsRevoked)
            {
                session.IsRevoked = true;
                await db.SaveChangesAsync();
            }
        }

        DeleteSessionCookie(httpResponse, sessionSettings.CookieName);
        return Results.Ok(ApiResponse<object>.SuccessResponse(null, ApiMessages.Success));
    }

    private static async Task<IResult> GetActiveSessionsAsync(
        ClaimsPrincipal userClaims,
        AppDbContext db)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return Results.Json(
                ApiResponse<object>.FailureResponse(ApiMessages.Unauthorized),
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var now = DateTime.UtcNow;

        var activeSessions = await db.UserSessions
            .Where(x => x.UserId == userId &&
                        !x.IsRevoked &&
                        x.ExpiresAtUtc > now &&
                        x.AbsoluteExpiresAtUtc > now)
            .OrderByDescending(x => x.LastAccessedAtUtc)
            .Select(x => new
            {
                x.Id,
                x.IpAddress,
                x.UserAgent,
                x.LastAccessedAtUtc,
                x.CreatedAtUtc,
                x.ExpiresAtUtc
            })
            .ToListAsync();

        return Results.Ok(ApiResponse<object>.SuccessResponse(activeSessions, ApiMessages.Success));
    }

    private static async Task<IResult> RevokeSessionAsync(
        long sessionId,
        ClaimsPrincipal userClaims,
        AppDbContext db)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return Results.Json(
                ApiResponse<object>.FailureResponse(ApiMessages.Unauthorized),
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var session = await db.UserSessions.FirstOrDefaultAsync(x => x.Id == sessionId && x.UserId == userId);
        if (session is null)
        {
            return Results.Json(
                ApiResponse<object>.FailureResponse(ApiMessages.SessionNotFound),
                statusCode: StatusCodes.Status404NotFound);
        }

        if (!session.IsRevoked)
        {
            session.IsRevoked = true;
            await db.SaveChangesAsync();
        }

        return Results.Ok(ApiResponse<object>.SuccessResponse(null, ApiMessages.Success));
    }

    private static async Task<IResult> RevokeAllSessionsAsync(
        ClaimsPrincipal userClaims,
        AppDbContext db)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return Results.Json(
                ApiResponse<object>.FailureResponse(ApiMessages.Unauthorized),
                statusCode: StatusCodes.Status401Unauthorized);
        }

        await db.UserSessions
            .Where(x => x.UserId == userId && !x.IsRevoked)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsRevoked, true));

        return Results.Ok(ApiResponse<object>.SuccessResponse(null, ApiMessages.Success));
    }

    private static async Task<IResult> RevokeOtherSessionsAsync(
        ClaimsPrincipal userClaims,
        HttpRequest httpRequest,
        ISessionTokenService sessionTokenService,
        IOptions<AppSettings> appSettings,
        AppDbContext db)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return Results.Json(
                ApiResponse<object>.FailureResponse(ApiMessages.Unauthorized),
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var sessionSettings = appSettings.Value.Session;

        if (!httpRequest.Cookies.TryGetValue(sessionSettings.CookieName, out var currentToken) ||
            string.IsNullOrWhiteSpace(currentToken))
        {
            return Results.Json(
                ApiResponse<object>.FailureResponse(ApiMessages.Unauthorized),
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var currentSessionHash = sessionTokenService.Hash(currentToken);

        await db.UserSessions
            .Where(x => x.UserId == userId && !x.IsRevoked && x.SessionHash != currentSessionHash)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsRevoked, true));

        return Results.Ok(ApiResponse<object>.SuccessResponse(null, ApiMessages.Success));
    }

    private static string GetClientIp(HttpRequest request)
    {
        if (request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor) &&
            !string.IsNullOrWhiteSpace(forwardedFor))
        {
            return forwardedFor.ToString().Split(',')[0].Trim();
        }

        return request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    private static string GetUserAgent(HttpRequest request)
    {
        var userAgent = request.Headers.UserAgent.ToString();
        return string.IsNullOrWhiteSpace(userAgent) ? "Unknown" : userAgent;
    }

    private static void SetSessionCookie(
        HttpResponse response,
        string token,
        TimeSpan lifetime,
        string cookieName)
    {
        response.Cookies.Append(
            cookieName,
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                MaxAge = lifetime,
                IsEssential = true
            });
    }

    private static void DeleteSessionCookie(HttpResponse response, string cookieName)
    {
        response.Cookies.Delete(
            cookieName,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            });
    }
}