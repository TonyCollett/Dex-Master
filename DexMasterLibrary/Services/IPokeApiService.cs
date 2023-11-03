using Type = PokeApiNet.Type;
using Version = PokeApiNet.Version;

namespace DexMasterLibrary.Services;

public interface IPokeApiService
{
    public Task<Ability> GetAbilityByIdAsync(int id);
    public Task<Ability> GetAbilityByNameAsync(string name);
    public Task<Generation> GetGenerationByNameAsync(string name);
    public Task<IEnumerable<Ability>> GetPokemonAbilitiesAsync(Pokemon pokemon);
    public Task<IEnumerable<Generation>> GetGenerationListAsync();
    public Task<IEnumerable<Move>> GetPokemonMovesAsync(Pokemon pokemon);
    public Task<(int, Dictionary<Pokemon, PokemonSpecies>)> FilterPokemonListAsync(int limit, int offset, Pokedex pokedex, string searchTerm = "");
    public Task<(int, IEnumerable<Pokemon>)> GetPokemonListAsync(int limit, int offset);
    public Task<IEnumerable<PokemonSpecies>> GetPokemonSpeciesListAsync(int limit, int offset);
    public Task<IEnumerable<Pokedex>> GetPokedexListAsync();
    public Task<Pokedex> GetPokedexByNameAsync(string pokedex);
    public Task<Pokedex> GetPokedexByIdAsync(int pokedexId);
    public Task<IEnumerable<Type>> GetPokemonTypesAsync(Pokemon pokemon);
    public Task<IEnumerable<Type>> GetTypeListAsync();
    public Task<Move> GetMoveByIdAsync(int id);
    public Task<Move> GetMoveByNameAsync(string name);
    public Task<Pokemon> GetPokemonByIdAsync(int id);
    public Task<Pokemon> GetPokemonByNameAsync(string name);
    public Task<PokemonSpecies> GetPokemonSpeciesAsync(Pokemon pokemon);
    public Task<Type> GetTypeByIdAsync(int id);
    public Task<Type> GetTypeByNameAsync(string name);
    public Task<Version> GetVersionByNameAsync(string name);
    public Task<VersionGroup> GetVersionGroupByNameAsync(string name);
    public Task<IEnumerable<VersionGroup>> GetVersionGroupListAsync();
    public Task<IEnumerable<Version>> GetAllVersionsOfGenerationAsync(Generation generation);
}