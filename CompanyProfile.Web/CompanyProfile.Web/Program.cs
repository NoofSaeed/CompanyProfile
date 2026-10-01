using CompanyProfile.Web.Authentication;
using CompanyProfile.Web.Components;
using CompanyProfile.Web.Client.Services;
using GenericRestHelper.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();



builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<SessionCookieProvider>();

builder.Services.AddGenericRestClientWithCookie<SessionCookieProvider>(
    client =>
    {
        var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]
            ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

        client.BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/");
    },(provider, cancellationToken) => provider.GetCookieAsync(cancellationToken));

builder.Services.AddScoped<CompanyApiClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CompanyProfile.Web.Client._Imports).Assembly);

app.Run();
