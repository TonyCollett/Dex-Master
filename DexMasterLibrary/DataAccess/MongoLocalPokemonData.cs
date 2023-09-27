namespace DexMasterLibrary.DataAccess;

public class MongoLocalPokemonData : ILocalPokemonData
{
    private readonly IDbConnection _db;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<LocalPokemon> _localPokemonCollection;
    private const string CacheName = "LocalPokemonData";

    public MongoLocalPokemonData(IDbConnection db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
        _localPokemonCollection = db.LocalPokemonCollection;
    }

    public async Task<DTPagedResult<LocalPokemon>> GetActivePagedResultsAsync(
        int pageNumber,
        int localPokemonsPerPage,
        string searchTerm = "",
        string createdById = "")
    {
        string cacheKey = $"{CacheName}_Page{pageNumber}_Size{localPokemonsPerPage}_Search{searchTerm}_CreatedBy{createdById}";
        string totalCountCacheKey = $"{CacheName}_TotalCount_Search{searchTerm}_CreatedBy{createdById}";

        var output = _cache.Get<List<LocalPokemon>>(cacheKey);
        var cachedTotalCount = _cache.Get<long?>(totalCountCacheKey);

        if (output is not null && cachedTotalCount.HasValue)
        {
            int totalPages = (int)Math.Ceiling((double)cachedTotalCount.Value / localPokemonsPerPage);
            return new DTPagedResult<LocalPokemon>
            {
                Items = output,
                TotalPages = totalPages,
            };
        }

        FilterDefinition<LocalPokemon>? filter = Builders<LocalPokemon>.Filter.Gt(t => t.DateCreated, DateTime.MinValue);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchFilter = Builders<LocalPokemon>.Filter.Regex(p => p.Title, new BsonRegularExpression(searchTerm, "i"))
                            | Builders<LocalPokemon>.Filter.Regex(p => p.Description, new BsonRegularExpression(searchTerm, "i"));
            filter &= searchFilter;
        }

        if (!string.IsNullOrWhiteSpace(createdById))
        {
            var createdByFilter = Builders<LocalPokemon>.Filter.Eq(p => p.CreatedBy.Id, createdById);
            filter &= createdByFilter;
        }

        var totalCount = await _localPokemonCollection.CountDocumentsAsync(filter);
        var query = _localPokemonCollection.Find(filter);

        var results = await query.Skip((pageNumber - 1) * localPokemonsPerPage)
                                 .Limit(localPokemonsPerPage)
                                 .ToListAsync();

        int calculatedTotalPages = (int)Math.Ceiling((double)totalCount / localPokemonsPerPage);

        _cache.Set(cacheKey, results, TimeSpan.FromMinutes(1));
        _cache.Set(totalCountCacheKey, totalCount, TimeSpan.FromMinutes(1));

        return new DTPagedResult<LocalPokemon>
        {
            Items = results,
            TotalPages = calculatedTotalPages,
        };
    }

    public async Task<LocalPokemon> GetLocalPokemonByIdAsync(string id)
    {
        IAsyncCursor<LocalPokemon> results = await _localPokemonCollection.FindAsync(t => t.Id == id);
        return results.FirstOrDefault();
    }

    public async Task<LocalPokemon> GetRandomLocalPokemonAsync()
    {
        // Count the total number of documents in the collection
        long totalCount = await _localPokemonCollection.CountDocumentsAsync(FilterDefinition<LocalPokemon>.Empty);

        // If there are no documents, return null
        if (totalCount == 0)
        {
            return null;
        }

        // Generate a random index
        Random random = new Random();
        int randomIndex = random.Next(0, (int)totalCount);

        // Fetch the document at the random index
        var result = await _localPokemonCollection.Find(FilterDefinition<LocalPokemon>.Empty)
                                            .Skip(randomIndex)
                                            .Limit(1)
                                            .FirstOrDefaultAsync();
        return result;
    }

    public async Task UpdateLocalPokemonAsync(LocalPokemon localPokemon)
    {
        await _localPokemonCollection.ReplaceOneAsync(p => p.Id == localPokemon.Id, localPokemon);
        _cache.Remove(CacheName);
    }

    public async Task CreateLocalPokemonAsync(LocalPokemon localPokemon)
    {
        await CreateMultipleLocalPokemonsAsync(new[] { localPokemon });
    }

    public async Task CreateMultipleLocalPokemonsAsync(IEnumerable<LocalPokemon> localPokemons)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var localPokemonsInTransaction = db.GetCollection<LocalPokemon>(_db.LocalPokemonCollectionName);
            await localPokemonsInTransaction.InsertManyAsync(session, localPokemons);

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

    public async Task DeleteLocalPokemonAsync(LocalPokemon localPokemon)
    {
        await _localPokemonCollection.DeleteOneAsync(p => p.Id == localPokemon.Id);
        _cache.Remove(CacheName);
    }
}

