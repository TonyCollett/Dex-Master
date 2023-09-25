namespace DexMasterLibrary.DataAccess;

public class MongoPromptData : IPromptData
{
    private readonly IDbConnection _db;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Prompt> _promptCollection;
    private const string CacheName = "PromptData";

    public MongoPromptData(IDbConnection db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
        _promptCollection = db.PromptCollection;
    }

    public async Task<List<Prompt>> GetAllActivePromptsAsync()
    {
        var output = _cache.Get<List<Prompt>>(CacheName);

        if (output is not null)
        {
            return output;
        }

        var results = await _promptCollection.FindAsync(t => t.Status == Enums.Status.Active);
        output = results.ToList();

        _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));

        return output;
    }

    public async Task<List<Prompt>> GetPromptsByIdsAsync(IEnumerable<string> promptIds)
    {
        var allPrompts = await GetAllActivePromptsAsync();
        return allPrompts.FindAll(p => promptIds.Contains(p.Id));
    }

    public async Task ToggleFavouriteOnPromptAsync(Prompt prompt, string userId)
    {
        if (prompt is null)
        {
            throw new SocketException();
        }

        if (prompt.FavouritedBy.Contains(userId))
        {
            prompt.FavouritedBy.Remove(userId);
        }
        else
        {
            prompt.FavouritedBy.Add(userId);
        }

        await _promptCollection.ReplaceOneAsync(p => p.Id == prompt.Id, prompt);
    }

    public async Task<DTPagedResult<Prompt>> GetActivePagedResultsAsync(
        int pageNumber,
        int promptsPerPage,
        string searchTerm = "",
        SortOption? sortOption = SortOption.Default,
        string createdById = "")
    {
        string cacheKey = $"{CacheName}_Page{pageNumber}_Size{promptsPerPage}_Search{searchTerm}_Sort{sortOption}_CreatedBy{createdById}";
        string totalCountCacheKey = $"{CacheName}_TotalCount_Search{searchTerm}_CreatedBy{createdById}";

        var output = _cache.Get<List<Prompt>>(cacheKey);
        var cachedTotalCount = _cache.Get<long?>(totalCountCacheKey);

        if (output is not null && cachedTotalCount.HasValue)
        {
            int totalPages = (int)Math.Ceiling((double)cachedTotalCount.Value / promptsPerPage);
            return new DTPagedResult<Prompt>
            {
                Items = output,
                TotalPages = totalPages,
            };
        }

        var filter = Builders<Prompt>.Filter.Eq(t => t.Status, Status.Active);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchFilter = Builders<Prompt>.Filter.Regex(p => p.Title, new BsonRegularExpression(searchTerm, "i"))
                            | Builders<Prompt>.Filter.Regex(p => p.PromptDescription, new BsonRegularExpression(searchTerm, "i"))
                            | Builders<Prompt>.Filter.Regex(p => p.PromptResult, new BsonRegularExpression(searchTerm, "i"));
            filter &= searchFilter;
        }

        if (!string.IsNullOrWhiteSpace(createdById))
        {
            var createdByFilter = Builders<Prompt>.Filter.Eq(p => p.CreatedBy.Id, createdById);
            filter &= createdByFilter;
        }

        var totalCount = await _promptCollection.CountDocumentsAsync(filter);
        var query = _promptCollection.Find(filter);

        if (sortOption.HasValue)
        {
            query = sortOption.Value switch
            {
                SortOption.MostViewed => query.SortByDescending(p => p.ViewCount),
                SortOption.MostFavourited => query.SortByDescending(p => p.FavouritedBy.Count),
                SortOption.New => query.SortByDescending(p => p.DateCreated),
                _ => query.SortBy(p => p.DateCreated),
            };
        }

        var results = await query.Skip((pageNumber - 1) * promptsPerPage)
                                 .Limit(promptsPerPage)
                                 .ToListAsync();

        int calculatedTotalPages = (int)Math.Ceiling((double)totalCount / promptsPerPage);

        _cache.Set(cacheKey, results, TimeSpan.FromMinutes(1));
        _cache.Set(totalCountCacheKey, totalCount, TimeSpan.FromMinutes(1));

        return new DTPagedResult<Prompt>
        {
            Items = results,
            TotalPages = calculatedTotalPages,
        };
    }

    public async Task<Prompt> GetPromptByIdAsync(string id)
    {
        IAsyncCursor<Prompt> results = await _promptCollection.FindAsync(t => t.Id == id);
        return results.FirstOrDefault();
    }

    public async Task<Prompt> GetRandomPromptAsync()
    {
        // Count the total number of documents in the collection
        long totalCount = await _promptCollection.CountDocumentsAsync(FilterDefinition<Prompt>.Empty);

        // If there are no documents, return null
        if (totalCount == 0)
        {
            return null;
        }

        // Generate a random index
        Random random = new Random();
        int randomIndex = random.Next(0, (int)totalCount);

        // Fetch the document at the random index
        var result = await _promptCollection.Find(FilterDefinition<Prompt>.Empty)
                                            .Skip(randomIndex)
                                            .Limit(1)
                                            .FirstOrDefaultAsync();
        return result;
    }


    public async Task IncreaseViewCountForPromptAsync(Prompt prompt)
    {
        prompt.ViewCount++;
        await _promptCollection.ReplaceOneAsync(p => p.Id == prompt.Id, prompt);
        _cache.Remove(CacheName);
    }

    public async Task SharePromptAsync(Prompt prompt)
    {
        prompt.ShareCount++;
        await _promptCollection.ReplaceOneAsync(p => p.Id == prompt.Id, prompt);
        _cache.Remove(CacheName);
    }

    public async Task UpdatePromptAsync(Prompt prompt)
    {
        await _promptCollection.ReplaceOneAsync(p => p.Id == prompt.Id, prompt);
        _cache.Remove(CacheName);
    }

    public async Task CreatePromptAsync(Prompt prompt)
    {
        await CreateMultiplePromptsAsync(new[] { prompt });
    }

    public async Task CreateMultiplePromptsAsync(IEnumerable<Prompt> prompts)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var promptsInTransaction = db.GetCollection<Prompt>(_db.PromptCollectionName);
            await promptsInTransaction.InsertManyAsync(session, prompts);

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

    public async Task DeletePromptAsync(Prompt prompt)
    {
        await _promptCollection.DeleteOneAsync(p => p.Id == prompt.Id);
        _cache.Remove(CacheName);
    }
}

