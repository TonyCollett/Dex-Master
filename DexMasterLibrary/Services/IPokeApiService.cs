﻿using DexMasterUI.DTClasses;
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
    public Task<IEnumerable<PokeApiNet.Version>> GetVersionListAsync();
    public Task<IEnumerable<Move>> GetPokemonMovesAsync(Pokemon pokemon);
    public Task<string> GetMoveMachineNameAsync(Move move);
    public Task<IEnumerable<DTPokemon>> GetPokemonSpeciesPageFromPokedexAsync(int limit, int offset, Pokedex pokedex);
    public Task<IEnumerable<DTPokemon>> SearchPokemonSpeciesPageFromPokedexAsync(int limit, int offset, Pokedex pokedex, string searchTerm = "");
    public Task<DTPokemon> GetPokemonAsync(string? name = null, int? id = null, string? varietyName = "");
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
    public Task<IEnumerable<Pokemon>> GetPokemonVarietiesAsync(PokemonSpecies pokemonSpecies);
    public Task<IEnumerable<PokemonSpecies>> GetAllPokemonSpeciesAsync();
    public Task<PokemonSpecies> GetPokemonSpeciesByNameAsync(string pokemonName);
    public Task<Type> GetTypeByIdAsync(int id);
    public Task<Type> GetTypeByNameAsync(string name);
    public Task<Version> GetVersionByNameAsync(string name);
    public Task<Version> GetVersionByIdAsync(int id);
    public Task<VersionGroup> GetVersionGroupByNameAsync(string name);
    public Task<IEnumerable<VersionGroup>> GetVersionGroupListAsync();
    public Task<IEnumerable<Version>> GetAllVersionsOfGenerationAsync(Generation generation);
    public Task<IEnumerable<Item>> GetItemListAsync();
    public Task<EvolutionChain> GetEvolutionChainAsync(PokemonSpecies pokemonSpecies);
}