namespace DexMasterLibrary.DataAccess.Interfaces;

public interface ILocalPokemonData
{
    Task<DTPagedResult<LocalPokemon>> GetActivePagedResultsAsync(
        int pageNumber,
        int localPokemonsPerPage,
        string searchTerm = "",
        string createdById = "");
    Task<LocalPokemon> GetLocalPokemonByIdAsync(string id);
    Task<LocalPokemon> GetRandomLocalPokemonAsync();
    Task UpdateLocalPokemonAsync(LocalPokemon localPokemon);
    Task CreateLocalPokemonAsync(LocalPokemon localPokemon);
    Task CreateMultipleLocalPokemonsAsync(IEnumerable<LocalPokemon> localPokemons);
    Task DeleteLocalPokemonAsync(LocalPokemon localPokemon);
}