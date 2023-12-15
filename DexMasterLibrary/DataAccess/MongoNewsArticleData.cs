namespace DexMasterLibrary.DataAccess;

public class MongoNewsArticleData : INewsArticleData
{
    private readonly IMongoCollection<NewsArticle> _newsArticles;
    private readonly IMemoryCache _cache;
    
    private const string CacheKey = "NewsArticle";
    private const string CacheKeyAll = "NewsArticle_ALL";

    public MongoNewsArticleData(IDbConnection db, IMemoryCache cache)
    {
        _newsArticles = db.NewsArticleCollection;
        _cache = cache;
    }

    public async Task<List<NewsArticle>?> GetAllNewsArticlesAsync()
    {
        if (!_cache.TryGetValue(CacheKeyAll, out List<NewsArticle>? cachedNewsArticles))
        {
            cachedNewsArticles = await _newsArticles.Find(_ => true).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            _cache.Set(CacheKeyAll, cachedNewsArticles, cacheEntryOptions);
        }
        return cachedNewsArticles;
    }

    public async Task<NewsArticle?> GetNewsArticleByIdAsync(string id)
    {
        var cacheKey = $"{CacheKey}_{id}";
        if (!_cache.TryGetValue(cacheKey, out NewsArticle? newsArticle))
        {
            newsArticle = await _newsArticles.Find(n => n.Id == id).FirstOrDefaultAsync();
            if (newsArticle != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(15)); // Cache for 15 minutes
                _cache.Set(cacheKey, newsArticle, cacheEntryOptions);
            }
        }
        return newsArticle;
    }

    public async Task CreateNewsArticleAsync(NewsArticle newsArticle)
    {
        await _newsArticles.InsertOneAsync(newsArticle);
        _cache.Remove(CacheKeyAll);
    }

    public async Task UpdateNewsArticleAsync(NewsArticle newsArticle)
    {
        var cacheKey = $"{CacheKey}_{newsArticle.Id}";
        var filter = Builders<NewsArticle>.Filter.Eq("Id", newsArticle.Id);
        _cache.Remove(CacheKeyAll);
        _cache.Remove(cacheKey);
        await _newsArticles.ReplaceOneAsync(filter, newsArticle);
    }
    
    public async Task DeleteNewsArticleAsync(string id)
    {
        var cacheKey = $"{CacheKey}_{id}";
        var filter = Builders<NewsArticle>.Filter.Eq("Id", id);
        await _newsArticles.DeleteOneAsync(filter);
        _cache.Remove(CacheKeyAll);
        _cache.Remove(cacheKey);
    }
}