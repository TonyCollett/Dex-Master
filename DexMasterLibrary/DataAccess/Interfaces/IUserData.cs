namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IUserData
{
    Task CreateUserAsync(User user);
    Task CreateMultipleUsersAsync(IEnumerable<User> users);
    Task<User> GetUserByBsonIdAsync(string id);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserFromAuthentication(string objectId);
    Task<List<User>> GetAllUsersAsync(bool ignoreCache = false);
    Task ToggleFavouriteOnUserAsync(User user, string promptId);
    Task UpdateUserAsync(User user);
    Task<bool> CheckUsernameExists(string username);
}