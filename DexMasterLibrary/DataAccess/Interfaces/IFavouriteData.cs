namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IFavouriteData
{
    Task<List<Favourite>> GetAllFavouritesAsync();
    Task<bool> ToggleFavouriteAsync(string promptId, string userId);
    Task<List<Favourite>> GetFavouritesByPromptIdAsync(string promptId);
    Task<List<Favourite>> GetFavouritesByUserIdAsync(string userId);
    Task<bool> IsPromptFavouriteByPromptIdAndUserIdAsync(string promptId, string userId);
    Task CreateFavouriteAsync(Favourite favourite);
    Task CreateMultipleFavouritesAsync(IEnumerable<Favourite> favourites);
    Task<ReplaceOneResult> UpdateFavouriteAsync(Favourite favourite);
}