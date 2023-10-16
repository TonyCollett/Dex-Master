namespace DexMasterLibrary.DataAccess.Interfaces;

public interface ICustomPokemonDetailsData
{
    /// <summary>
    /// Retrieves a CustomPokemonDetails from the data store by its ID.
    /// </summary>
    /// <param name="id">The ID of the CustomPokemonDetails to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved CustomPokemonDetails.</returns>
    public Task<CustomPokemonDetails?> GetCustomPokemonDetailsByIdAsync(int id);
    
    /// <summary>
    /// Update CustomPokemonDetails
    /// </summary>
    public Task UpsertCustomPokemonDetailsAsync(CustomPokemonDetails pokemonDetails);
}