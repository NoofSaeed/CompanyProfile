using CompanyProfile.Api.Services;
using CompanyProfile.Core.DTOs.Auth;
using CompanyProfile.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CompanyProfile.Api.Endpoints;

public static class AuthEndpoints
{
    private const string RefreshTokenCookieName = "__Host-refreshToken";

    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication");

        group.MapPost("/login", LoginAsync)
            .AllowAnonymous()
            .WithName("Login");

        return app;
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        HttpResponse httpResponse)
    {
        var user = await userManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            return Results.Unauthorized();
        }

        var passwordValid = await userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!passwordValid)
        {
            return Results.Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);

        var (accessToken, expiresAtUtc) =
            await jwtService.GenerateTokenAsync(
                user.Id,
                user.UserName ?? request.UserName,
                user.Email,
                roles);

        var refreshToken = refreshTokenService.GenerateToken(user.Id);

        httpResponse.Cookies.Append(
            RefreshTokenCookieName,
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                MaxAge = TimeSpan.FromDays(7)
            });

        var response = new LoginResponse(
            accessToken,
            expiresAtUtc);

        return Results.Ok(response);
    }
}