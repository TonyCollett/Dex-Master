using PokeApiNet;

namespace DexMasterLibrary.DataAccess;

public class MongoLocalPokemonData : ILocalPokemonData
{
    private readonly IDbConnection _db;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Pokemon> _localPokemonCollection;
    private const string CacheName = "LocalPokemonData";

    public MongoLocalPokemonData(IDbConnection db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
        _localPokemonCollection = db.LocalPokemonCollection;
    }

    /// <inheritdoc />
    public async Task<DTPagedResult<Pokemon>> GetActivePagedResultsAsync(
        int pageNumber,
        int localPokemonsPerPage)
    {
        string cacheKey = $"{CacheName}_Page{pageNumber}_Size{localPokemonsPerPage}";
        string totalCountCacheKey = $"{CacheName}_TotalCount";

        var output = _cache.Get<List<Pokemon>>(cacheKey);
        var cachedTotalCount = _cache.Get<long?>(totalCountCacheKey);

        if (output is not null && cachedTotalCount.HasValue)
        {
            int totalPages = (int)Math.Ceiling((double)cachedTotalCount.Value / localPokemonsPerPage);
            return new DTPagedResult<Pokemon>
            {
                Items = output,
                TotalPages = totalPages,
            };
        }

        FilterDefinition<Pokemon>? filter = Builders<Pokemon>.Filter.Gt(t => t.Id, 0);
        
        var totalCount = await _localPokemonCollection.CountDocumentsAsync(filter);
        var query = _localPokemonCollection.Find(filter);

        var results = await query.Skip((pageNumber - 1) * localPokemonsPerPage)
                                 .Limit(localPokemonsPerPage)
                                 .ToListAsync();

        int calculatedTotalPages = (int)Math.Ceiling((double)totalCount / localPokemonsPerPage);

        _cache.Set(cacheKey, results, TimeSpan.FromMinutes(1));
        _cache.Set(totalCountCacheKey, totalCount, TimeSpan.FromMinutes(1));

        return new DTPagedResult<Pokemon>
        {
            Items = results,
            TotalPages = calculatedTotalPages,
        };
    }

    /// <inheritdoc />
    public async Task<Pokemon> GetLocalPokemonByIdAsync(int id)
    {
        IAsyncCursor<Pokemon> results = await _localPokemonCollection.FindAsync(t => t.Id == id);
        return results.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<Pokemon> GetRandomLocalPokemonAsync()
    {
        // Count the total number of documents in the collection
        long totalCount = await _localPokemonCollection.CountDocumentsAsync(FilterDefinition<Pokemon>.Empty);

        // If there are no documents, return null
        if (totalCount == 0)
        {
            return null;
        }

        // Generate a random index
        Random random = new Random();
        int randomIndex = random.Next(0, (int)totalCount);

        // Fetch the document at the random index
        var result = await _localPokemonCollection.Find(FilterDefinition<Pokemon>.Empty)
                                            .Skip(randomIndex)
                                            .Limit(1)
                                            .FirstOrDefaultAsync();
        return result;
    }
    
    /// <inheritdoc />
    public async Task<long> GetCountAsync()
    {
        return await _localPokemonCollection.CountDocumentsAsync(FilterDefinition<Pokemon>.Empty);
    }
    
    /// <inheritdoc />
    public async Task UpdateLocalPokemonAsync(Pokemon localPokemon)
    {
        await _localPokemonCollection.ReplaceOneAsync(p => p.Id == localPokemon.Id, localPokemon);
        _cache.Remove(CacheName);
    }

    /// <inheritdoc />
    public async Task UpdateMultipleLocalPokemonsAsync(IEnumerable<Pokemon> pokemonList)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var localPokemonsInTransaction = db.GetCollection<Pokemon>(_db.LocalPokemonCollectionName);
            
            // replace with bulkwrite
            foreach (var pokemon in pokemonList)
            {
                await localPokemonsInTransaction.ReplaceOneAsync(session, p => p.Id == pokemon.Id, pokemon);
            }

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

    public async Task CreateMultipleLocalPokemonsAsync(IEnumerable<Pokemon> pokemonList)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var localPokemonsInTransaction = db.GetCollection<Pokemon>(_db.LocalPokemonCollectionName);
            await localPokemonsInTransaction.InsertManyAsync(session, pokemonList);

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

