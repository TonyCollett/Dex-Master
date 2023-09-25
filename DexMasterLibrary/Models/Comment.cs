namespace DexMasterLibrary.Models;
public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ParentId { get; set; }
    public string CommentText { get; set; }
    public User CommentedBy { get; set; }
    public string CommentedByName { get => CommentedBy.DisplayName; }
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public bool Archived { get; set; }
}
