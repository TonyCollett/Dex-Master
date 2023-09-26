namespace DexMasterUI.Services;

public interface IPokeApiService
{
    public Task<IEnumerable<Pokemon>> GetPokemonListAsync(int limit, int offset);
}