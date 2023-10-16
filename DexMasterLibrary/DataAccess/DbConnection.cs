using Microsoft.Extensions.Configuration;

namespace DexMasterLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly string _connectionId = "MongoDB";
    
    public string DbName { get; }
    public string UserCollectionName { get; } = "users";
    public string AdditionalPokemonCollectionName { get; } = "pokemon_custom";
    public MongoClient Client { get; }
    public IMongoCollection<User> UserCollection { get; }
    public IMongoCollection<CustomPokemonDetails> CustomPokemonDetailsCollection { get; }

    public DbConnection(IConfiguration config)
    {
        try
        {
            Client = new MongoClient(config.GetConnectionString(_connectionId));
            DbName = config["DatabaseName"] ?? "PokeCloud";
            
            IMongoDatabase db = Client.GetDatabase(DbName);
            UserCollection = db.GetCollection<User>(UserCollectionName);
            CustomPokemonDetailsCollection = db.GetCollection<CustomPokemonDetails>(AdditionalPokemonCollectionName);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}

