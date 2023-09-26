namespace DexMasterUI.Services;

public class PokeApiService : IPokeApiService
{
    private PokeApiClient Client { get; } = new();
    
    public async Task<IEnumerable<Pokemon>> GetPokemonListAsync(int limit, int offset)
    {
        var pokemonsPage = await Client.GetNamedResourcePageAsync<Pokemon>(limit, offset);

        return await Client.GetResourceAsync<Pokemon>(pokemonsPage.Results);
    }
}