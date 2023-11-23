namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IUserData
{
    Task CreateUserAsync(User user);
    Task CreateMultipleUsersAsync(IEnumerable<User> users);
    Task<User> GetUserByIdAsync(string id);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserFromAuthentication(string objectId);
    Task<List<User>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task UpdateUsernameByIdAsync(string userId, string username);
    Task UserLoggedInAsync(User user);
    Task<bool> CheckUsernameExists(string username);
}