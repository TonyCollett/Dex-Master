using DexMasterUI.Services;

namespace DexMasterLibrary.DataAccess;

public class MongoPokemonSpeciesData : IPokemonSpeciesData
{
    private readonly IDbConnection _db;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<PokemonSpecies> _pokemonSpeciesCollection;
    private const string CacheName = "PokemonSpeciesData";
    private readonly PokeApiClient _pokeApiClient = new();
    private readonly PokeApiService _pokeApiService = new();

    public MongoPokemonSpeciesData(IDbConnection db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
        _pokemonSpeciesCollection = db.PokemonSpeciesCollection;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<PokemonSpecies>?> GetPokemonSpeciesListAsync(int limit, int offset)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<PokemonSpecies>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var storedPokemonSpecies = await _pokemonSpeciesCollection.FindAsync(FilterDefinition<PokemonSpecies>.Empty);

            if (storedPokemonSpecies != null && storedPokemonSpecies.Any())
            {
                output = await storedPokemonSpecies.ToListAsync();
            }
            else
            {
                var apiPokemonSpecies  = await _pokeApiService.GetPokemonSpeciesListAsync(limit, offset);
                output = apiPokemonSpecies.ToList();
            }
            
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(60));
        }

        return output.GetRange(offset, limit);
    }

    /// <inheritdoc />
    public async Task<PokemonSpecies> GetPokemonSpeciesByIdAsync(int id)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<PokemonSpecies>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var foundPokemonSpecies = await _pokemonSpeciesCollection.FindAsync(p => p.Id == id);
            return foundPokemonSpecies.FirstOrDefault();
        }
        else
        {
            return output.First(p => p.Id == id);
        }
    }
    
    /// <inheritdoc />
    public async Task UpdatePokemonSpeciesAsync(PokemonSpecies pokemonSpecies)
    {
        await _pokemonSpeciesCollection.ReplaceOneAsync(p => p.Id == pokemonSpecies.Id, pokemonSpecies);
        _cache.Remove(CacheName);
    }

    /// <inheritdoc />
    public async Task CreateMultiplePokemonSpeciesAsync(IEnumerable<PokemonSpecies> pokemonSpeciesList, bool deleteExisting = false)
    {
        MongoClient client = _db.Client;
        using IClientSessionHandle? session = await client.StartSessionAsync();

        try
        {
            session.StartTransaction();
            
            IMongoDatabase? db = client.GetDatabase(_db.DbName);
            
            if (deleteExisting)
            {
                await db.DropCollectionAsync(_db.PokemonSpeciesCollectionName);
            }
            
            var pokemonSpeciesInTransaction = db.GetCollection<PokemonSpecies>(_db.PokemonSpeciesCollectionName);
            await pokemonSpeciesInTransaction.InsertManyAsync(session, pokemonSpeciesList);

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

