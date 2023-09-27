using PokeApiNet;

namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IDbConnection
{
    MongoClient Client { get; }
    string DbName { get; }
    IMongoCollection<Pokemon> LocalPokemonCollection { get; }
    string LocalPokemonCollectionName { get; }
    IMongoCollection<User> UserCollection { get; }
    string UserCollectionName { get; }
}