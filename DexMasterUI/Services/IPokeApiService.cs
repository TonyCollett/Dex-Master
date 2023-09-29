using Type = PokeApiNet.Type;

namespace DexMasterUI.Services;

public interface IPokeApiService
{
    public Task<IEnumerable<Pokemon>> GetPokemonListAsync(int limit, int offset);
    public Task<IEnumerable<Ability>> GetPokemonAbilitiesAsync(Pokemon pokemon);
    public Task<IEnumerable<Move>> GetPokemonMovesAsync(Pokemon pokemon);
    public Task<IEnumerable<Type>> GetPokemonTypesAsync(Pokemon pokemon);
    public Task<PokemonSpecies> GetPokemonSpeciesAsync(Pokemon pokemon);
    public Task<Pokemon> GetPokemonByNameAsync(string name);
    public Task<Pokemon> GetPokemonByIdAsync(int id);
    public Task<Ability> GetAbilityByNameAsync(string name);
    public Task<Ability> GetAbilityByIdAsync(int id);
    public Task<Move> GetMoveByNameAsync(string name);
    public Task<Move> GetMoveByIdAsync(int id);
}