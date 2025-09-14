using Solvation.Algorithms;
using Solvation.Services;
using DotNetEnv;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")
    ?? throw new InvalidOperationException("Environment variable 'FRONTEND_URL' is not set.");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IHandService, HandService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Solver.VerifyInteractions();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors();

app.Run();