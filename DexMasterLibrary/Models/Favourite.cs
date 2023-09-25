namespace DexMasterLibrary.Models;

public class Favourite
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string PromptId { get; set; }
    public string UserId { get; set; }
    public DateTime FavouritedDate { get; set; } = DateTime.UtcNow;
}
