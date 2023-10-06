namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IPokemonData
{
    /// <summary>
    /// Retrieves a list of Pokemon from the data source.
    /// </summary>
    /// <param name="limit">The maximum number of Pokemon to retrieve.</param>
    /// <param name="offset">The number of Pokemon to skip before beginning to retrieve.</param>
    /// <returns>An asynchronous operation that returns an enumerable collection of Pokemon, or null if no Pokemon are found.</returns>
    public Task<IEnumerable<Pokemon>?> GetPokemonListAsync(int limit, int offset);
    
    /// <summary>
    /// Retrieves a Pokemon from the data store by its ID.
    /// </summary>
    /// <param name="id">The ID of the Pokemon to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved Pokemon.</returns>
    public Task<Pokemon> GetPokemonByIdAsync(int id);
    
    /// <summary>
    /// Update Pokemon
    /// </summary>
    public Task UpdatePokemonAsync(Pokemon pokemon);
    
    /// <summary>
    /// Create Pokemon
    /// </summary>
    public Task CreateMultiplePokemonAsync(IEnumerable<Pokemon> pokemonList, bool deleteExisting = false);

}