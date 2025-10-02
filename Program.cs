using cse325_team7_project.Components;
using cse325_team7_project.Api.Services;
using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Domain.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Web API controllers
builder.Services.AddControllers(options =>
{
    // Bind Mongo ObjectId automatically from route/query to parameter type ObjectId
    options.ModelBinderProviders.Insert(0, new cse325_team7_project.Api.Binders.ObjectIdModelBinderProvider());
});

// Mongo collections registration 
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var cs = config["Mongo:ConnectionString"] ?? "mongodb://localhost:27017";
    return new MongoClient(cs);
});
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var dbName = config["Mongo:Database"] ?? "moviehub";
    return sp.GetRequiredService<IMongoClient>().GetDatabase(dbName);
});
builder.Services.AddSingleton<IMongoCollection<Movie>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Movie>("movies"));
builder.Services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("users"));
builder.Services.AddSingleton<IMongoCollection<MoviesList>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<MoviesList>("lists"));

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMoviesListService, MoviesListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// global error handling middleware
app.UseMiddleware<cse325_team7_project.Api.Middleware.ErrorHandlingMiddleware>();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
