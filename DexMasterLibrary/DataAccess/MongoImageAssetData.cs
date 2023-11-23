namespace DexMasterLibrary.DataAccess;

public class MongoImageAssetData : IImageAssetData
{
    private readonly IMongoCollection<ImageAsset> _imageAssets;

    public MongoImageAssetData(IDbConnection db)
    {
        _imageAssets = db.ImageAssetCollection;
    }

    public async Task<ImageAsset> GetImageAssetByIdAsync(string id) 
        => await _imageAssets.Find(img => img.Id == id).FirstOrDefaultAsync();

    public async Task<ImageAsset> GetImageAssetByNameAsync(string name) 
        => await _imageAssets.Find(img => img.Name == name).FirstOrDefaultAsync();

    public async Task UpsertImageAssetAsync(ImageAsset imageAsset)
        => await _imageAssets.ReplaceOneAsync(img => img.Id == imageAsset.Id, imageAsset, new ReplaceOptions { IsUpsert = true });
    
    
    public async Task SaveMultipleImageAssetAsync(IEnumerable<ImageAsset> imageAssets)
        => await _imageAssets.InsertManyAsync(imageAssets);

}