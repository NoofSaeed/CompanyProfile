using CompanyProfile.Api;
using CompanyProfile.Api.Authentication;
using CompanyProfile.Api.Endpoints;
using CompanyProfile.Api.Services;
using CompanyProfile.Core.Entities;
using CompanyProfile.Infrastructure.Data;
using CompanyProfile.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

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
#region Database

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=../CompanyProfile.Infrastructure/company.db"));
#endregion
#region Identity

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
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
    .AddScheme<AuthenticationSchemeOptions, CustomSessionAuthenticationHandler>
    ("CustomSessionScheme", null);
builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection("AppSettings"))
    .ValidateOnStart();

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


app.Run();
#endregion