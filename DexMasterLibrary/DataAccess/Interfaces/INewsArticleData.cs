namespace DexMasterLibrary.DataAccess.Interfaces;

public interface INewsArticleData
{
    Task<List<NewsArticle>?> GetAllNewsArticlesAsync();
    Task<NewsArticle?> GetNewsArticleByIdAsync(string id);
    Task CreateNewsArticleAsync(NewsArticle newsArticle);
    Task UpdateNewsArticleAsync(NewsArticle newsArticle);
    Task DeleteNewsArticleAsync(string id);
}