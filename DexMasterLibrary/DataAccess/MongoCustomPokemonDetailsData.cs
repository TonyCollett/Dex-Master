namespace DexMasterLibrary.DataAccess;

public class MongoCustomPokemonDetailsData : ICustomPokemonDetailsData
{
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<CustomPokemonDetails> _customPokemonDetailsCollection;
    private const string CacheName = "CustomPokemonDetailsData";

    public MongoCustomPokemonDetailsData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _customPokemonDetailsCollection = db.CustomPokemonDetailsCollection;
    }

    /// <inheritdoc />
    public async Task<CustomPokemonDetails?> GetCustomPokemonDetailsByIdAsync(int id)
    {
        string cacheKey = $"{CacheName}-{id}";
        var output = _cache.Get<CustomPokemonDetails>(cacheKey);

        if (output == null)
        {
            var foundAdditionalPokemon = await _customPokemonDetailsCollection.FindAsync(p => p.PokemonId == id);
            output = foundAdditionalPokemon.FirstOrDefault();
            _cache.Set(cacheKey, output);
        }

        return output;
    }
    
    /// <inheritdoc />
    public async Task UpsertCustomPokemonDetailsAsync(CustomPokemonDetails customPokemonDetails)
    {
        await _customPokemonDetailsCollection.ReplaceOneAsync(
            p => p.Id == customPokemonDetails.Id, 
            customPokemonDetails, 
            new ReplaceOptions(){IsUpsert = true});
        _cache.Remove(CacheName);
    }
}

