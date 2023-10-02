﻿using PokeApiNet;

namespace DexMasterLibrary.DataAccess;

public class MongoPokemonData : IPokemonData
{
    private readonly IDbConnection _db;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Pokemon> _pokemonCollection;
    private const string CacheName = "PokemonData";
    private readonly PokeApiClient _pokeApiClient = new();

    public MongoPokemonData(IDbConnection db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
        _pokemonCollection = db.PokemonCollection;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Pokemon>?> GetPokemonListAsync(int limit, int offset)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<Pokemon>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var foundPokemon = await _pokeApiClient.GetNamedResourcePageAsync<Pokemon>(limit, offset);
            output = await _pokeApiClient.GetResourceAsync<Pokemon>(foundPokemon.Results);
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(60));
        }

        return output.GetRange(offset, limit);
    }

    /// <inheritdoc />
    public async Task<Pokemon> GetPokemonByIdAsync(int id)
    {
        string cacheKey = $"{CacheName}";
        var output = _cache.Get<List<Pokemon>>(cacheKey);

        if (output == null || output.Count == 0)
        {
            var foundPokemon = await _pokemonCollection.FindAsync(p => p.Id == id);
            return foundPokemon.FirstOrDefault();
        }
        else
        {
            return output.First(p => p.Id == id);
        }
    }

    /// <inheritdoc />
    public async Task<Pokemon?> GetRandomPokemonAsync()
    {
        // Count the total number of documents in the collection
        long totalCount = await _pokemonCollection.CountDocumentsAsync(FilterDefinition<Pokemon>.Empty);

        // If there are no documents, return null
        if (totalCount == 0)
        {
            return null;
        }

        // Generate a random index
        Random random = new Random();
        int randomIndex = random.Next(0, (int)totalCount);

        // Fetch the document at the random index
        var result = await _pokemonCollection.Find(FilterDefinition<Pokemon>.Empty)
                                            .Skip(randomIndex)
                                            .Limit(1)
                                            .FirstOrDefaultAsync();
        return result;
    }
    
    /// <inheritdoc />
    public async Task UpdatePokemonAsync(Pokemon pokemon)
    {
        await _pokemonCollection.ReplaceOneAsync(p => p.Id == pokemon.Id, pokemon);
        _cache.Remove(CacheName);
    }

    /// <inheritdoc />
    public async Task CreateMultiplePokemonAsync(IEnumerable<Pokemon> pokemonList, bool deleteExisting = false)
    {
        MongoClient client = _db.Client;
        using IClientSessionHandle? session = await client.StartSessionAsync();

        try
        {
            session.StartTransaction();
            
            IMongoDatabase? db = client.GetDatabase(_db.DbName);
            var pokemonInTransaction = db.GetCollection<Pokemon>(_db.PokemonCollectionName);
            
            if (deleteExisting)
            {
                await pokemonInTransaction.DeleteManyAsync(session, FilterDefinition<Pokemon>.Empty);
            }
            
            await pokemonInTransaction.InsertManyAsync(session, pokemonList);

            await session.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            throw;
        }
        finally
        {
            _cache.Remove(CacheName);
        }
    }
}

