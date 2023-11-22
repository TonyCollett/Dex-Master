namespace DexMasterLibrary.DataAccess;

public class MongoImageAssetData : IImageAssetData
{
    private readonly IMongoCollection<ImageAsset> _imageAssets;

    public MongoImageAssetData(IDbConnection db)
    {
        _imageAssets = db.ImageAssetCollection; // Assuming ImageAssetCollection is defined in IDbConnection
    }

    public async Task<ImageAsset> GetImageAssetByIdAsync(string id) => await _imageAssets.Find(img => img.Id == id).FirstOrDefaultAsync();

    public async Task<ImageAsset> GetImageAssetByNameAsync(string name) => await _imageAssets.Find(img => img.Name == name).FirstOrDefaultAsync();

    public async Task SaveImageAssetAsync(ImageAsset imageAsset)
    {
        await _imageAssets.ReplaceOneAsync(
            img => img.Id == imageAsset.Id,
            imageAsset,
            new ReplaceOptions { IsUpsert = true }
        );
    }
}