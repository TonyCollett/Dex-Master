namespace DexMasterLibrary.DataAccess;

public class MongoBasicPokemonData : IBasicPokemonData
{
    private readonly IDbConnection _db;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<BasicPokemon> _basicPokemonCollection;
    private const string CacheName = "BasicPokemonData";

    public MongoBasicPokemonData(IDbConnection db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
        _basicPokemonCollection = db.BasicPokemonCollection;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicPokemon>?> GetBasicPokemonListAsync(int limit, int offset)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<BasicPokemon>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var foundBasicPokemon = await _basicPokemonCollection.FindAsync(FilterDefinition<BasicPokemon>.Empty);
            output = foundBasicPokemon.ToList();
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(60));
        }

        return output.GetRange(offset, limit);
    }

    /// <inheritdoc />
    public async Task<BasicPokemon> GetBasicPokemonByIdAsync(string id)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<BasicPokemon>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var foundBasicPokemon = await _basicPokemonCollection.FindAsync(p => p.Id == id);
            return foundBasicPokemon.FirstOrDefault();
        }
        else
        {
            return output.First(p => p.Id == id);
        }
    }
    
    /// <inheritdoc />
    public async Task UpdateBasicPokemonAsync(BasicPokemon pokemon)
    {
        await _basicPokemonCollection.ReplaceOneAsync(p => p.Id == pokemon.Id, pokemon);
        _cache.Remove(CacheName);
    }

    /// <inheritdoc />
    public async Task CreateMultipleBasicPokemonAsync(IEnumerable<BasicPokemon> pokemonList, bool deleteExisting = false)
    {
        MongoClient client = _db.Client;
        using IClientSessionHandle? session = await client.StartSessionAsync();

        try
        {
            session.StartTransaction();
            
            IMongoDatabase? db = client.GetDatabase(_db.DbName);
            
            if (deleteExisting)
            {
                await db.DropCollectionAsync(_db.DbName);
            }
            
            var pokemonInTransaction = db.GetCollection<BasicPokemon>(_db.BasicPokemonCollectionName);
            await pokemonInTransaction.InsertManyAsync(session, pokemonList);

            await session.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            throw;
        }
        finally
        {
            _cache.Remove(CacheName);
        }
    }
}

