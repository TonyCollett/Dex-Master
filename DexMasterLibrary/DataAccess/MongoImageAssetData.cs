namespace DexMasterLibrary.DataAccess;

public class MongoImageAssetData : IImageAssetData
{
    private readonly IMongoCollection<ImageAsset?> _imageAssets;
    private readonly IMemoryCache _cache;

    public MongoImageAssetData(IDbConnection db, IMemoryCache cache)
    {
        _imageAssets = db.ImageAssetCollection;
        _cache = cache;
    }

    public async Task<ImageAsset?> GetImageAssetByIdAsync(string id)
    {
        // Check if the asset is already in the cache
        if (!_cache.TryGetValue(id, out ImageAsset? imageAsset))
        {
            imageAsset = await _imageAssets.Find(img => img != null && img.Id == id).FirstOrDefaultAsync();

            // If the image asset is found, cache it
            if (imageAsset != null)
            {
                // Set cache options, e.g., absolute expiration time
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(1)); 

                _cache.Set(id, imageAsset, cacheEntryOptions);
            }
        }

        return imageAsset;
    }

    public async Task<ImageAsset?> GetImageAssetByNameAsync(string name) 
    {
        // Check if the asset is already in the cache
        if (!_cache.TryGetValue(name, out ImageAsset? imageAsset))
        {
            imageAsset = await _imageAssets.Find(img => img != null && img.Name == name).FirstOrDefaultAsync();

            // If the image asset is found, cache it
            if (imageAsset != null)
            {
                // Set cache options, e.g., absolute expiration time
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(1)); 

                _cache.Set(name, imageAsset, cacheEntryOptions);
            }
        }

        return imageAsset;
    }

    public async Task UpsertImageAssetAsync(ImageAsset? imageAsset)
        => await _imageAssets.ReplaceOneAsync(img => img != null && imageAsset != null && img.Id == imageAsset.Id, imageAsset, new ReplaceOptions { IsUpsert = true });
    
    public async Task SaveMultipleImageAssetAsync(IEnumerable<ImageAsset?> imageAssets)
        => await _imageAssets.InsertManyAsync(imageAssets);

}