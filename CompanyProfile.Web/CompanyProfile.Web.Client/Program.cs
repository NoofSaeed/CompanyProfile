using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CompanyProfile.Web.Client.Services;
using GenericRestHelper.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddGenericRestClient(client =>
{
	var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]
		?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

	client.BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/");
}).AddHttpMessageHandler<BrowserCredentialsHandler>();

builder.Services.AddScoped<CompanyApiClient>();
builder.Services.AddTransient<BrowserCredentialsHandler>();

await builder.Build().RunAsync();
