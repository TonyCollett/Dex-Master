namespace DexMasterLibrary.Models;

public class Prompt
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string PromptDescription { get; set; }
    public string PromptResult { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public User CreatedBy { get; set; }
    public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;
    public User LastUpdatedBy { get; set; }
    public PromptType PromptType { get; set; }
    public Status Status { get; set; }
    public int ViewCount { get; set; }
    public int ShareCount { get; set; }
    public bool HasAttachments { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public List<string> FavouritedBy { get; set; } = new List<string>();
}
