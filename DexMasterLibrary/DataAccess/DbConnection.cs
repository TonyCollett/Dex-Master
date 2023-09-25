// Ignore Spelling: Favourite

using Microsoft.Extensions.Configuration;

namespace DexMasterLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private readonly string _connectionId = "MongoDB";
    public string DbName { get; }
    public string CommentCollectionName { get; } = "comments";
    public string FavouriteCollectionName { get; } = "favourites";
    public string UserCollectionName { get; } = "users";
    public string PromptCollectionName { get; } = "prompts";
    public MongoClient Client { get; }
    public IMongoCollection<Comment> CommentCollection { get; }
    public IMongoCollection<Favourite> FavouriteCollection { get; }
    public IMongoCollection<User> UserCollection { get; }
    public IMongoCollection<Prompt> PromptCollection { get; }

    public DbConnection(IConfiguration config)
    {
        try
        {
            _config = config;
            Client = new MongoClient(_config.GetConnectionString(_connectionId));
            DbName = _config["DatabaseName"] ?? "PokeCloud";
            _db = Client.GetDatabase(DbName);

            FavouriteCollection = _db.GetCollection<Favourite>(FavouriteCollectionName);
            CommentCollection = _db.GetCollection<Comment>(CommentCollectionName);
            UserCollection = _db.GetCollection<User>(UserCollectionName);
            PromptCollection = _db.GetCollection<Prompt>(PromptCollectionName);
        }
        catch (System.Exception)
        {

            throw;
        }

    }

    public async Task DropAllDataCollectionsAsync()
    {
        await _db.DropCollectionAsync(FavouriteCollectionName);
        await _db.DropCollectionAsync(CommentCollectionName);
        await _db.DropCollectionAsync(UserCollectionName);
        await _db.DropCollectionAsync(PromptCollectionName);
    }
}

