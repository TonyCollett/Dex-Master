namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IDbConnection
{
    MongoClient Client { get; }
    string DbName { get; }
    IMongoCollection<Pokemon> PokemonCollection { get; }
    string PokemonCollectionName { get; }
    IMongoCollection<BasicPokemon> BasicPokemonCollection { get; }
    string BasicPokemonCollectionName { get; }
    IMongoCollection<PokemonSpecies> PokemonSpeciesCollection { get; }
    string PokemonSpeciesCollectionName { get; }
    IMongoCollection<User> UserCollection { get; }
    string UserCollectionName { get; }
}