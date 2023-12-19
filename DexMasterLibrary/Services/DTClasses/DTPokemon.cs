namespace DexMasterUI.DTClasses;

public class DTPokemon
{
    public PokemonSpecies PokemonSpecies { get; set; }
    public IEnumerable<Pokemon> PokemonVarieties { get; set; }
}