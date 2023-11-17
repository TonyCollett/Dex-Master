namespace DexMasterLibrary.DataAccess;

public class MongoNewsArticleData : INewsArticleData
{
    private readonly IMongoCollection<NewsArticle> _newsArticles;

    public MongoNewsArticleData(IDbConnection db)
    {
        _newsArticles = db.NewsArticleCollection;
    }

    public async Task<List<NewsArticle>> GetAllNewsArticlesAsync()
    {
        var results = await _newsArticles.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<NewsArticle> GetNewsArticleByIdAsync(string id)
    {
        var results = await _newsArticles.FindAsync(n => n.Id == id);
        return await results.FirstOrDefaultAsync();
    }

    public async Task CreateNewsArticleAsync(NewsArticle newsArticle)
    {
        await _newsArticles.InsertOneAsync(newsArticle);
    }

    public async Task UpdateNewsArticleAsync(NewsArticle newsArticle)
    {
        var filter = Builders<NewsArticle>.Filter.Eq("Id", newsArticle.Id);
        await _newsArticles.ReplaceOneAsync(filter, newsArticle);
    }
}