namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IUserData
{
    /// <summary>
    /// Create a new user
    /// </summary>
    Task CreateUserAsync(User user);
    
    /// <summary>
    /// Create multiple users
    /// </summary>
    Task CreateMultipleUsersAsync(IEnumerable<User> users);
    
    /// <summary>
    /// Get a user by their ID
    /// </summary>
    Task<User> GetUserByIdAsync(string id);
    
    /// <summary>
    /// Get a user by their username
    /// </summary>
    Task<User> GetUserByUsernameAsync(string username);
    
    /// <summary>
    /// Get user from the AuthenticationStateProvider
    /// </summary>
    Task<User> GetUserFromAuthentication(string objectId);
    
    /// <summary>
    /// Get all users
    /// </summary>
    Task<List<User>> GetAllUsersAsync();
    
    /// <summary>
    /// Update a user
    /// </summary>
    Task UpdateUserAsync(User user);
    
    /// <summary>
    /// Update a user's username by their ID
    /// </summary>
    Task UpdateUsernameByIdAsync(string userId, string username);
    
    /// <summary>
    /// Update a user's last logged in date
    /// </summary>
    Task UserLoggedInAsync(User user);
    
    /// <summary>
    /// Check if a username exists
    /// </summary>
    Task<bool> CheckUsernameExists(string username);
}