using Microsoft.Extensions.Configuration;

namespace DexMasterLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly string _connectionId = "MongoDB";
    
    public string DbName { get; }
    public string UserCollectionName { get; } = "users";
    public string PokemonCollectionName { get; } = "pokemon";
    public string BasicPokemonCollectionName { get; } = "basic-pokemon";
    public string PokemonSpeciesCollectionName { get; } = "pokemon-species";
    public MongoClient Client { get; }
    public IMongoCollection<User> UserCollection { get; }
    public IMongoCollection<Pokemon> PokemonCollection { get; }
    public IMongoCollection<BasicPokemon> BasicPokemonCollection { get; }
    public IMongoCollection<PokemonSpecies> PokemonSpeciesCollection { get; }

    public DbConnection(IConfiguration config)
    {
        try
        {
            Client = new MongoClient(config.GetConnectionString(_connectionId));
            DbName = config["DatabaseName"] ?? "PokeCloud";
            
            IMongoDatabase db = Client.GetDatabase(DbName);
            UserCollection = db.GetCollection<User>(UserCollectionName);
            PokemonCollection = db.GetCollection<Pokemon>(PokemonCollectionName);
            PokemonSpeciesCollection = db.GetCollection<PokemonSpecies>(PokemonSpeciesCollectionName);
            BasicPokemonCollection = db.GetCollection<BasicPokemon>(BasicPokemonCollectionName);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}

