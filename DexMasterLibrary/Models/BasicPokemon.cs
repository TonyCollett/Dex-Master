namespace DexMasterLibrary.Models;

public class BasicPokemon
{
    public string Name { get; set; }
    public PokemonType Type1 { get; set; }
    public PokemonType? Type2 { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}