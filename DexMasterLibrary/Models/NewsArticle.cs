namespace DexMasterLibrary.Models;

public class NewsArticle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public DateTime DatePosted { get; set; }
    public string CreatedBy { get; set; }
    public string DetailedDescription { get; set; }
    public string Excerpt { get; set; }
    public string Category { get; set; }
}