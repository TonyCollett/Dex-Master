namespace DexMasterLibrary.DataAccess.Interfaces;

public interface ICommentData
{
    Task<List<Comment>> GetAllCommentsAsync();
    Task<List<Comment>> GetCommentsByParentIdAsync(string parentId);
    Task CreateCommentAsync(Comment category);
    Task CreateMultipleCommentsAsync(IEnumerable<Comment> comments);
    Task<ReplaceOneResult> UpdateCommentAsync(Comment category);
}