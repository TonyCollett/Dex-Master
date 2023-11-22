namespace DexMasterLibrary.DataAccess;

public class MongoNewsArticleData : INewsArticleData
{
    private readonly IMongoCollection<NewsArticle> _newsArticles;

    public MongoNewsArticleData(IDbConnection db)
    {
        _newsArticles = db.NewsArticleCollection;
    }

    public async Task<List<NewsArticle>> GetAllNewsArticlesAsync() => await _newsArticles.Find(_ => true).ToListAsync();

    public async Task<NewsArticle> GetNewsArticleByIdAsync(string id) => await _newsArticles.Find(n => n.Id == id).FirstOrDefaultAsync();

    public async Task CreateNewsArticleAsync(NewsArticle newsArticle)
    {
        await _newsArticles.InsertOneAsync(newsArticle);
    }

    public async Task UpdateNewsArticleAsync(NewsArticle newsArticle)
    {
        var filter = Builders<NewsArticle>.Filter.Eq("Id", newsArticle.Id);
        await _newsArticles.ReplaceOneAsync(filter, newsArticle);
    }
    
    public async Task DeleteNewsArticleAsync(string id)
    {
        var filter = Builders<NewsArticle>.Filter.Eq("Id", id);
        await _newsArticles.DeleteOneAsync(filter);
    }
}