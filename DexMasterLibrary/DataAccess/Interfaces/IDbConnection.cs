namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IDbConnection
{
    MongoClient Client { get; }
    string DbName { get; }
    IMongoCollection<CustomPokemonDetails> CustomPokemonDetailsCollection { get; }
    string AdditionalPokemonCollectionName { get; }
    IMongoCollection<User> UserCollection { get; }
    string UserCollectionName { get; }
}