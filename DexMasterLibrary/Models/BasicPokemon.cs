using DexMasterLibrary.Enums;

namespace DexMasterLibrary.Models;

public class BasicPokemon
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int NationalDexNumber { get; set; }
    public string Name { get; set; }
    public Types Type1 { get; set; }
    public Types? Type2 { get; set; }
    public string BasicImageSpriteUrl { get; set; }
}