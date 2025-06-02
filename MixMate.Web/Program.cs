using MixMate.Web;
using MixMate.Web.Components;
using MixMate.Web.Interfaces;
using MixMate.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

//Dependency injection
builder.Services.RegisterServices();
builder.Services
    .AddMixMateGraphQLClient()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5165/graphql/");
    });

builder.Services.AddScoped<IMixMateClient, MixMateClient>();

builder.AddServiceDefaults();

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