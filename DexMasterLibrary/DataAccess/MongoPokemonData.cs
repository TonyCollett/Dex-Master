using DexMasterUI.Services;

namespace DexMasterLibrary.DataAccess;

public class MongoPokemonData : IPokemonData
{
    private readonly IDbConnection _db;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Pokemon> _pokemonCollection;
    private const string CacheName = "PokemonData";
    private readonly PokeApiClient _pokeApiClient = new();
    private readonly PokeApiService _pokeApiService = new();

    public MongoPokemonData(IDbConnection db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
        _pokemonCollection = db.PokemonCollection;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Pokemon>?> GetPokemonListAsync(int limit, int offset)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<Pokemon>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var storedPokemon = await _pokemonCollection.FindAsync(FilterDefinition<Pokemon>.Empty);

            if (storedPokemon != null && storedPokemon.Any())
            {
                output = await storedPokemon.ToListAsync();
            }
            else
            {
                var apiPokemon  = await _pokeApiService.GetPokemonListAsync(limit, offset);
                output = apiPokemon.ToList();
            }
            
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(60));
        }

        return output.GetRange(offset, limit);
    }

    /// <inheritdoc />
    public async Task<Pokemon> GetPokemonByIdAsync(int id)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<Pokemon>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var foundPokemon = await _pokemonCollection.FindAsync(p => p.Id == id);
            return foundPokemon.FirstOrDefault();
        }
        else
        {
            return output.First(p => p.Id == id);
        }
    }
    
    /// <inheritdoc />
    public async Task UpdatePokemonAsync(Pokemon pokemon)
    {
        await _pokemonCollection.ReplaceOneAsync(p => p.Id == pokemon.Id, pokemon);
        _cache.Remove(CacheName);
    }

    /// <inheritdoc />
    public async Task CreateMultiplePokemonAsync(IEnumerable<Pokemon> pokemonList, bool deleteExisting = false)
    {
        MongoClient client = _db.Client;
        using IClientSessionHandle? session = await client.StartSessionAsync();

        try
        {
            session.StartTransaction();
            
            IMongoDatabase? db = client.GetDatabase(_db.DbName);
            
            if (deleteExisting)
            {
                await db.DropCollectionAsync(_db.PokemonCollectionName);
            }
            
            var pokemonInTransaction = db.GetCollection<Pokemon>(_db.PokemonCollectionName);
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

