using System.Diagnostics;
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
    
    public async Task<IEnumerable<PokemonSpecies>> GetPokemonSpeciesPageFromPokedexAsync(int limit, int offset, Pokedex pokedex)
    {
        return await Client.GetResourceAsync<PokemonSpecies>(pokedex.PokemonEntries.Skip(offset).Take(limit).Select(p => p.PokemonSpecies));
    }
    
    public async Task<IEnumerable<PokemonSpecies>> SearchPokemonSpeciesPageFromPokedexAsync(int limit, int offset, Pokedex pokedex, string searchTerm = "")
    {
        var pokemonSearchList = pokedex.PokemonEntries.Where(p => p.PokemonSpecies.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        
        return await Client.GetResourceAsync<PokemonSpecies>(pokemonSearchList.Skip(offset).Take(limit).Select(p => p.PokemonSpecies));
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
    
    public async Task<IEnumerable<Pokemon>> GetPokemonVarietiesAsync(PokemonSpecies pokemonSpecies)
    {
        return await Client.GetResourceAsync<Pokemon>(pokemonSpecies.Varieties.Select(v => v.Pokemon));
    }
    
    public async Task<IEnumerable<PokemonSpecies>> GetAllPokemonSpeciesAsync()
    {
        List<PokemonSpecies> pokemonSpeciesList = new List<PokemonSpecies>();
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        await foreach (var pokemonSpecies in Client.GetAllNamedResourcesAsync<PokemonSpecies>())
        {
            pokemonSpeciesList.Add(await Client.GetResourceAsync(pokemonSpecies));
        }
        stopwatch.Stop();
        Console.WriteLine($"Time taken to get all Pokemon Species: {stopwatch.ElapsedMilliseconds}ms");
        
        return pokemonSpeciesList;
    }
    
    public async Task<PokemonSpecies> GetPokemonSpeciesByNameAsync(string pokemonName)
    {
        return await Client.GetResourceAsync<PokemonSpecies>(pokemonName);
    }
    
    public async Task<IEnumerable<Pokedex>> GetPokedexListAsync()
    {
        var pokedexPage = await Client.GetNamedResourcePageAsync<Pokedex>(100, 0);

        return await Client.GetResourceAsync<Pokedex>(pokedexPage.Results);
    }
    
    public async Task<Pokedex> GetPokedexByNameAsync(string pokedex)
    {
        return await Client.GetResourceAsync<Pokedex>(pokedex);
    }
    
    public async Task<Pokedex> GetPokedexByIdAsync(int pokedexId)
    {
        return await Client.GetResourceAsync<Pokedex>(pokedexId);
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
    
    public async Task<IEnumerable<VersionGroup>> GetVersionGroupListAsync()
    {
        var versionGroups = await Client.GetNamedResourcePageAsync<VersionGroup>(100, 0);

        return await Client.GetResourceAsync<VersionGroup>(versionGroups.Results);
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
    
    public async Task<IEnumerable<Item> > GetItemListAsync()
    {
        var items = await Client.GetNamedResourcePageAsync<Item>(500, 0);

        return await Client.GetResourceAsync<Item>(items.Results);
    }
    
    public async Task<EvolutionChain> GetEvolutionChainAsync(PokemonSpecies pokemonSpecies)
    {
        return await Client.GetResourceAsync<EvolutionChain>(pokemonSpecies.EvolutionChain);
    }
}