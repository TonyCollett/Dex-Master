namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IPokemonData
{
    /// <summary>
    /// Get Paged Results of Pokemon
    /// </summary>
    public Task<IEnumerable<Pokemon>?> GetPokemonListAsync(int limit, int offset);
    
    /// <summary>
    /// Get Pokemon by Id
    /// </summary>
    public Task<Pokemon> GetPokemonByIdAsync(int id);
    
    /// <summary>
    /// Get Random Pokemon
    /// </summary>
    public Task<Pokemon?> GetRandomPokemonAsync();
    
    /// <summary>
    /// Update Pokemon
    /// </summary>
    public Task UpdatePokemonAsync(Pokemon pokemon);
    
    /// <summary>
    /// Create Pokemon
    /// </summary>
    public Task CreateMultiplePokemonAsync(IEnumerable<Pokemon> pokemonList, bool deleteExisting = false);

}