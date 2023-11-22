namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IDbConnection
{
    MongoClient Client { get; }
    string DbName { get; }
    IMongoCollection<User> UserCollection { get; }
    string UserCollectionName { get; }
    IMongoCollection<NewsArticle> NewsArticleCollection { get; }
    string NewsArticleCollectionName { get; }
    IMongoCollection<ImageAsset> ImageAssetCollection { get; }
    string ImageAssetCollectionName { get; }
}