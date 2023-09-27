// Ignore Spelling: Favourite

using Microsoft.Extensions.Configuration;

namespace DexMasterLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private readonly string _connectionId = "MongoDB";
    public string DbName { get; }
    public string UserCollectionName { get; } = "users";
    public string LocalPokemonCollectionName { get; } = "localpokemon";
    public MongoClient Client { get; }
    public IMongoCollection<User> UserCollection { get; }
    public IMongoCollection<LocalPokemon> LocalPokemonCollection { get; }

    public DbConnection(IConfiguration config)
    {
        try
        {
            _config = config;
            Client = new MongoClient(_config.GetConnectionString(_connectionId));
            DbName = _config["DatabaseName"] ?? "PokeCloud";
            _db = Client.GetDatabase(DbName);
            
            UserCollection = _db.GetCollection<User>(UserCollectionName);
            LocalPokemonCollection = _db.GetCollection<LocalPokemon>(LocalPokemonCollectionName);
        }
        catch (System.Exception)
        {
            throw;
        }

    }
}

