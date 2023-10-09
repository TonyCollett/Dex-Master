namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IPokemonSpeciesData
{
    /// <summary>
    /// Retrieves a list of PokemonSpecies from the data source.
    /// </summary>
    /// <param name="limit">The maximum number of PokemonSpecies to retrieve.</param>
    /// <param name="offset">The number of PokemonSpecies to skip before beginning to retrieve.</param>
    /// <returns>An asynchronous operation that returns an enumerable collection of PokemonSpecies, or null if no PokemonSpecies are found.</returns>
    public Task<IEnumerable<PokemonSpecies>?> GetPokemonSpeciesListAsync(int limit, int offset);
    
    /// <summary>
    /// Retrieves a PokemonSpecies from the data store by its ID.
    /// </summary>
    /// <param name="id">The ID of the PokemonSpecies to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved PokemonSpecies.</returns>
    public Task<PokemonSpecies?> GetPokemonSpeciesByIdAsync(int id);
    
    /// <summary>
    /// Update PokemonSpecies
    /// </summary>
    public Task UpdatePokemonSpeciesAsync(PokemonSpecies pokemonSpecies);
    
    /// <summary>
    /// Create PokemonSpecies
    /// </summary>
    public Task CreateMultiplePokemonSpeciesAsync(IEnumerable<PokemonSpecies> pokemonSpeciesList, bool deleteExisting = false);

}