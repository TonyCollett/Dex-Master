using Type = PokeApiNet.Type;

namespace DexMasterUI.Services;

public class PokeApiService : IPokeApiService
{
    private PokeApiClient Client { get; } = new();
    
    public async Task<IEnumerable<Pokemon>> GetPokemonListAsync(int limit, int offset)
    {
        var pokemonPage = await Client.GetNamedResourcePageAsync<Pokemon>(limit, offset);

        return await Client.GetResourceAsync<Pokemon>(pokemonPage.Results);
    }
    
    public async Task<PokemonSpecies> GetPokemonSpeciesAsync(Pokemon pokemon)
    {
        return await Client.GetResourceAsync<PokemonSpecies>(pokemon.Species);
    }
    
    public async Task<IEnumerable<Ability>> GetPokemonAbilitiesAsync(Pokemon pokemon)
    {
        return await Client.GetResourceAsync<Ability>(pokemon.Abilities.Select(a => a.Ability));
    }

    public async Task<IEnumerable<Move>> GetPokemonMovesAsync(Pokemon pokemon)
    {
        return await Client.GetResourceAsync<Move>(pokemon.Moves.Select(m => m.Move));
    }

    public async Task<IEnumerable<Type>> GetPokemonTypesAsync(Pokemon pokemon)
    {
        return await Client.GetResourceAsync<Type>(pokemon.Types.Select(t => t.Type));
    }
    
    public async Task<Pokemon> GetPokemonByNameAsync(string name)
    {
        return await Client.GetResourceAsync<Pokemon>(name);
    }
    
    public async Task<Pokemon> GetPokemonByIdAsync(int id)
    {
        return await Client.GetResourceAsync<Pokemon>(id);
    }
    
    public async Task<Ability> GetAbilityByNameAsync(string name)
    {
        return await Client.GetResourceAsync<Ability>(name);
    }
    
    public async Task<Ability> GetAbilityByIdAsync(int id)
    {
        return await Client.GetResourceAsync<Ability>(id);
    }
    
    public async Task<Move> GetMoveByNameAsync(string name)
    {
        return await Client.GetResourceAsync<Move>(name);
    }
    
    public async Task<Move> GetMoveByIdAsync(int id)
    {
        return await Client.GetResourceAsync<Move>(id);
    }
    
    public async Task<Type> GetTypeByNameAsync(string name)
    {
        return await Client.GetResourceAsync<Type>(name);
    }
    
    public async Task<Type> GetTypeByIdAsync(int id)
    {
        return await Client.GetResourceAsync<Type>(id);
    }
}