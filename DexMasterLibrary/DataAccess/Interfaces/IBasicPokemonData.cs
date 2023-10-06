namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IBasicPokemonData
{
    /// <summary>
    /// Retrieves a list of BasicPokemon from the data source.
    /// </summary>
    /// <param name="limit">The maximum number of BasicPokemon to retrieve.</param>
    /// <param name="offset">The number of BasicPokemon to skip before beginning to retrieve.</param>
    /// <returns>An asynchronous operation that returns an enumerable collection of BasicPokemon, or null if no BasicPokemon are found.</returns>
    public Task<IEnumerable<BasicPokemon>?> GetBasicPokemonListAsync(int limit, int offset);
    
    /// <summary>
    /// Retrieves a BasicPokemon from the data store by its ID.
    /// </summary>
    /// <param name="id">The ID of the BasicPokemon to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved BasicPokemon.</returns>
    public Task<BasicPokemon> GetBasicPokemonByIdAsync(string id);
    
    /// <summary>
    /// Get Random BasicPokemon
    /// </summary>
    public Task<BasicPokemon?> GetRandomBasicPokemonAsync();
    
    /// <summary>
    /// Update BasicPokemon
    /// </summary>
    public Task UpdateBasicPokemonAsync(BasicPokemon pokemon);
    
    /// <summary>
    /// Create BasicPokemon
    /// </summary>
    public Task CreateMultipleBasicPokemonAsync(IEnumerable<BasicPokemon> pokemonList, bool deleteExisting = false);

}