using Microsoft.Extensions.DependencyInjection;
using MixMate.Web;
using MixMate.Web.Components;
using MixMate.Web.Interfaces;
using MixMate.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

//Dependency injection
builder.Services.RegisterServices();
builder.Services
    .AddMixMateGraphQLClient()
    .ConfigureHttpClient((serviceProvider, client) =>
    {
        client.BaseAddress = new Uri("http://localhost:5165/graphql/");
        // Log the resolved URL
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("GraphQL client base address: {BaseAddress}", client.BaseAddress);
    });

builder.Services.AddScoped<IMixMateClient, MixMateClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();