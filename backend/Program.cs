using Solvation.Algorithms;

Console.WriteLine("Variables :::");
Console.WriteLine(Environment.GetEnvironmentVariable("DATABASE_USER_USERNAME"));
Console.WriteLine(Environment.GetEnvironmentVariable("DATABASE_USER_PASSWORD"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<MongoDbService>();

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