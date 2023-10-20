using Type = PokeApiNet.Type;
using Version = PokeApiNet.Version;

namespace DexMasterLibrary.Services;

public class PokeApiService : IPokeApiService
{
    private PokeApiClient Client { get; } = new();
    
    public async Task<(int, IEnumerable<Pokemon>)> GetPokemonListAsync(int limit, int offset)
    {
        var pokemonPage = await Client.GetNamedResourcePageAsync<Pokemon>(limit, offset);

        return (pokemonPage.Count, await Client.GetResourceAsync<Pokemon>(pokemonPage.Results));
    }
    
    public async Task<(int, IEnumerable<PokemonSpecies>)> FilterPokemonListAsync(int limit, int offset, string searchTerm = "", string version = "")
    {
        Pokedex versionGroupPokemon;
        
        if (!string.IsNullOrWhiteSpace(version) && version != "national")
        {
            Version versionObject = await GetVersionByNameAsync(version);
            VersionGroup versionGroup = await GetVersionGroupByNameAsync(versionObject.VersionGroup.Name);

            versionGroupPokemon = await Client.GetResourceAsync(versionGroup.Pokedexes.First());
        }
        else
        {
            versionGroupPokemon = await Client.GetResourceAsync<Pokedex>(1);
        }

        var pokemonInPokedexList = versionGroupPokemon.PokemonEntries.Select(p => p.PokemonSpecies).ToList();
        
        var filteredPokemon = pokemonInPokedexList.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        var pokemonSpeciesResourceList = filteredPokemon.ToList();
        
        var pokemonSpeciesList = await Client.GetResourceAsync<PokemonSpecies>(pokemonSpeciesResourceList.Skip(offset).Take(limit).ToList());
        
        return (pokemonSpeciesResourceList.Count, pokemonSpeciesList);
    }

    public async Task<IEnumerable<PokemonSpecies>> GetPokemonSpeciesListAsync(int limit, int offset)
    {
        var pokemonSpeciesPage = await Client.GetNamedResourcePageAsync<PokemonSpecies>(limit, offset);

        return await Client.GetResourceAsync<PokemonSpecies>(pokemonSpeciesPage.Results);
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

    public async Task<IEnumerable<Type>> GetTypeListAsync()
    {
        var types = await Client.GetNamedResourcePageAsync<Type>();
        
        return await Client.GetResourceAsync<Type>(types.Results);
    }
    
    public async Task<Type> GetTypeByNameAsync(string name)
    {
        return await Client.GetResourceAsync<Type>(name);
    }
    
    public async Task<Type> GetTypeByIdAsync(int id)
    {
        return await Client.GetResourceAsync<Type>(id);
    }
    
    public async Task<Generation> GetGenerationByNameAsync(string name)
    {
        return await Client.GetResourceAsync<Generation>(name);
    }
    
    public async Task<IEnumerable<Generation>> GetGenerationListAsync()
    {
        var generations = await Client.GetNamedResourcePageAsync<Generation>();

        return await Client.GetResourceAsync<Generation>(generations.Results);
    }
    
    public async Task<IEnumerable<PokeApiNet.Version>> GetVersionListAsync()
    {
        var versions = await Client.GetNamedResourcePageAsync<PokeApiNet.Version>(50, 0);

        return await Client.GetResourceAsync<PokeApiNet.Version>(versions.Results);
    }
    
    public async Task<VersionGroup> GetVersionGroupByNameAsync(string name)
    {
        return await Client.GetResourceAsync<VersionGroup>(name);
    }
    
    public async Task<Version> GetVersionByNameAsync(string name)
    {
        return await Client.GetResourceAsync<Version>(name);
    }
    
    public async Task<Version> GetVersionByIdAsync(int id)
    {
        return await Client.GetResourceAsync<Version>(id);
    }

    public async Task<IEnumerable<Version>> GetAllVersionsOfGenerationAsync(Generation generation)
    {
        var versionGroups = await Client.GetResourceAsync<VersionGroup>(generation.VersionGroups);
        
        return await Client.GetResourceAsync<Version>(versionGroups.SelectMany(vg => vg.Versions));
    }
}