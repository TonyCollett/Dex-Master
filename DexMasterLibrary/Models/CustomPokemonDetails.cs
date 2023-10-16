namespace DexMasterLibrary.Models;

public class CustomPokemonDetails
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int PokemonId { get; set; }
}