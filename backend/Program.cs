using Solvation.Algorithms;

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

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    Solver.VerifyInteractions();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors();

app.Run();