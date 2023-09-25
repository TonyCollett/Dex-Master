namespace DexMasterLibrary.DataAccess;

public class MongoCommentData : ICommentData
{
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Comment> _comments;
    private const string CacheName = "CommentData";

    public MongoCommentData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _comments = db.CommentCollection;
    }

    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        var output = _cache.Get<List<Comment>>(CacheName);
        if (output is null)
        {
            var results = await _comments.FindAsync(_ => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromDays(1));
        }

        return output;
    }

    public async Task<List<Comment>> GetCommentsByParentIdAsync(string parentId)
    {
        var results = await _comments.FindAsync(x => x.ParentId == parentId);
        return results.ToList();
    }

    public async Task CreateCommentAsync(Comment comment)
    {
        await _comments.InsertOneAsync(comment);
    }

    public async Task CreateMultipleCommentsAsync(IEnumerable<Comment> comments)
    {
        await _comments.InsertManyAsync(comments);
    }

    public async Task<ReplaceOneResult> UpdateCommentAsync(Comment comment)
    {
        return await _comments.ReplaceOneAsync(x => x.Id == comment.Id, comment);
    }

}
