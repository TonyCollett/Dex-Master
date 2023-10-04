namespace DexMasterLibrary.Models;

public class UserPokemon
{
    public int PokemonId { get; set; }
    public bool Caught { get; set; }
    public bool CaughtShiny { get; set; }
    public bool Favourite { get; set; }
}