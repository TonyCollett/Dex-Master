namespace DexMasterLibrary.DataAccess;

public class MongoFavouriteData : IFavouriteData
{
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Favourite> _favourites;
    private const string CacheName = "FavouriteData";

    public MongoFavouriteData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _favourites = db.FavouriteCollection;
    }

    public async Task<List<Favourite>> GetAllFavouritesAsync()
    {
        var output = _cache.Get<List<Favourite>>(CacheName);
        if (output is null)
        {
            var results = await _favourites.FindAsync(_ => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromDays(1));
        }

        return output;
    }

    public async Task<bool> ToggleFavouriteAsync(string promptId, string userId)
    {
        var favourite = await _favourites.Find(x => x.PromptId == promptId && x.UserId == userId).FirstOrDefaultAsync();
        if (favourite is null)
        {
            favourite = new Favourite
            {
                PromptId = promptId,
                UserId = userId
            };
            await _favourites.InsertOneAsync(favourite);
            return true;
        }
        else
        {
            await _favourites.DeleteOneAsync(x => x.Id == favourite.Id);
            return false;
        }
    }

    public async Task<bool> IsPromptFavouriteByPromptIdAndUserIdAsync(string promptId, string userId)
    {
        var favourite = await _favourites.Find(x => x.PromptId == promptId && x.UserId == userId).FirstOrDefaultAsync();
        return favourite is not null;
    }

    public async Task<List<Favourite>> GetFavouritesByPromptIdAsync(string promptId)
    {
        var results = await _favourites.FindAsync(x => x.PromptId == promptId);
        return results.ToList();
    }

    public async Task<List<Favourite>> GetFavouritesByUserIdAsync(string userId)
    {
        var results = await _favourites.FindAsync(x => x.UserId == userId);
        return results.ToList();
    }

    public async Task CreateFavouriteAsync(Favourite favourite)
    {
        await _favourites.InsertOneAsync(favourite);
    }

    public async Task CreateMultipleFavouritesAsync(IEnumerable<Favourite> favourites)
    {
        await _favourites.InsertManyAsync(favourites);
    }

    public async Task<ReplaceOneResult> UpdateFavouriteAsync(Favourite favourite)
    {
        return await _favourites.ReplaceOneAsync(x => x.Id == favourite.Id, favourite);
    }

}
