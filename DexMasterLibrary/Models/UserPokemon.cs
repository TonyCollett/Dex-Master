namespace DexMasterLibrary.Models;

public class UserPokemon
{
    public Pokemon Pokemon { get; set; }
    public bool Caught { get; set; }
    public bool CaughtShiny { get; set; }
    public bool Favourite { get; set; }
}