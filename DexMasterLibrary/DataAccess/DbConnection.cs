using Microsoft.Extensions.Configuration;

namespace DexMasterLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly string _connectionId = "MongoDB";
    
    public string DbName { get; }
    public string UserCollectionName { get; } = "users";
    public string LocalPokemonCollectionName { get; } = "local-pokemon";
    public MongoClient Client { get; }
    public IMongoCollection<User> UserCollection { get; }
    public IMongoCollection<Pokemon> LocalPokemonCollection { get; }

    public DbConnection(IConfiguration config)
    {
        try
        {
            Client = new MongoClient(config.GetConnectionString(_connectionId));
            DbName = config["DatabaseName"] ?? "PokeCloud";
            
            IMongoDatabase db = Client.GetDatabase(DbName);
            UserCollection = db.GetCollection<User>(UserCollectionName);
            LocalPokemonCollection = db.GetCollection<Pokemon>(LocalPokemonCollectionName);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}

