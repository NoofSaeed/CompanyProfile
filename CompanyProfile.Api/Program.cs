using CompanyProfile.Api;
using CompanyProfile.Api.Authentication;
using CompanyProfile.Api.Endpoints;
using CompanyProfile.Api.Services;
using CompanyProfile.Infrastructure.Entities;
using CompanyProfile.Infrastructure.Data;
using CompanyProfile.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region Core

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion

#region Localization

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ar"),
        new CultureInfo("en")
    };

    options.DefaultRequestCulture = new RequestCulture("ar");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders =
    [
        new AcceptLanguageHeaderRequestCultureProvider()
    ];

    options.ApplyCurrentCultureToResponseHeaders = true;
});
#endregion

#region Database

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        "Data Source=../CompanyProfile.Infrastructure/company.db"));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<CompanyService>();
#endregion

#region Identity

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

#endregion

#region Validation

builder.Services.AddValidation();
#endregion

#region OpenAPI

builder.Services.AddOpenApi();

#endregion

#region Authorization

builder.Services.AddAuthorization();

#endregion

#region Application Services

builder.Services.AddDataProtection();

builder.Services.AddScoped<ISessionTokenService, SessionTokenService>();

builder.Services.AddAuthentication("CustomSessionScheme")
    .AddScheme<
        AuthenticationSchemeOptions,
        CustomSessionAuthenticationHandler>(
        "CustomSessionScheme",
        null);

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection("AppSettings"))
    .ValidateOnStart();

builder.Services.AddSingleton<ILanguageContext, LanguageContext>();

#endregion

var app = builder.Build();

#region Seed Data

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(services);
    await CompanyProfileSeeder.SeedAsync(services);
}

#endregion

#region Middleware

app.UseRequestLocalization();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region OpenAPI / Scalar

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

#endregion

#region Endpoints

app.MapCompanyEndpoints();
app.MapAuthEndpoints();

#endregion

app.Run();

