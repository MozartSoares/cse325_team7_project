using Microsoft.Extensions.Options;
using MovieHub7.Components;
using MovieHub7.Models;
using MovieHub7.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Registra serviços
builder.Services.AddSingleton<MovieService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<ListService>();

// Configura Razor Components com Interactive Server Rendering
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Seed de dados (opcional) — executa no startup
using (var scope = app.Services.CreateScope())
{
    var movieService = scope.ServiceProvider.GetRequiredService<MovieService>();
    await movieService.SeedAsync();
}

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();