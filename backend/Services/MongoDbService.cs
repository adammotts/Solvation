using MongoDB.Driver;
using DotNetEnv;
using Microsoft.Extensions.Configuration;

public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration configuration)
    {
        DotNetEnv.Env.Load();

        var username = Environment.GetEnvironmentVariable("DATABASE_USER_USERNAME")
            ?? throw new InvalidOperationException("Environment variable 'DATABASE_USER_USERNAME' is not set.");
        var password = Environment.GetEnvironmentVariable("DATABASE_USER_PASSWORD")
            ?? throw new InvalidOperationException("Environment variable 'DATABASE_USER_PASSWORD' is not set.");

        var connectionStringTemplate = configuration.GetSection("MongoDB:ConnectionString").Value
            ?? throw new InvalidOperationException("Configuration key 'MongoDB:ConnectionString' is not set.");
        var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
            ?? throw new InvalidOperationException("Configuration key 'MongoDB:DatabaseName' is not set.");

        var connectionString = connectionStringTemplate
            .Replace("<db_username>", username)
            .Replace("<db_password>", password);

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
