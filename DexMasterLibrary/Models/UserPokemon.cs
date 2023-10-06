namespace DexMasterLibrary.Models;

public class UserPokemon
{
    public BasicPokemon Pokemon { get; set; } = new ();
    public bool Caught { get; set; }
    public bool CaughtShiny { get; set; }
    public bool Favourite { get; set; }
}