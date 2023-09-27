using PokeApiNet;

namespace DexMasterLibrary.DataAccess.Interfaces;

public interface ILocalPokemonData
{
    /// <summary>
    /// Get Paged Results of LocalPokemon
    /// </summary>
    public Task<DTPagedResult<Pokemon>> GetPokemonListAsync(
        int limit,
        int offset);
    
    /// <summary>
    /// Get Pokemon by Id
    /// </summary>
    public Task<Pokemon> GetLocalPokemonByIdAsync(int id);
    
    /// <summary>
    /// Get Pokemon by Title
    /// </summary>
    public Task<Pokemon> GetRandomLocalPokemonAsync();
    
    /// <summary>
    /// Update Pokemon
    /// </summary>
    public Task UpdateLocalPokemonAsync(Pokemon localPokemon);
    
    /// <summary>
    /// Create LocalPokemon
    /// </summary>
    public Task CreateMultipleLocalPokemonsAsync(IEnumerable<Pokemon> pokemonList);

}