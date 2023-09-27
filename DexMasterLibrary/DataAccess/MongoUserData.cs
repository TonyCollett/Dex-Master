namespace DexMasterLibrary.DataAccess;

public class MongoUserData : IUserData
{
    private readonly IMongoCollection<User> _users;

    public MongoUserData(IDbConnection db)
    {
        _users = db.UserCollection;
    }

    public async Task<List<User>> GetAllUsersAsync(bool ignoreCache = false)
    {
        var results = await _users.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<User> GetUserByBsonIdAsync(string id)
    {
        var results = await _users.FindAsync(u => u.Id == id);
        return results.FirstOrDefault();
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var results = await _users.FindAsync(u => u.Username == username);
        return results.FirstOrDefault();
    }

    public async Task<User> GetUserFromAuthentication(string objectId)
    {
        var results = await _users.FindAsync(u => u.AuthenticationMethods.NameIdentifier == objectId);
        return results.FirstOrDefault();
    }

    public async Task CreateUserAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task CreateMultipleUsersAsync(IEnumerable<User> users)
    {
        await _users.InsertManyAsync(users);
    }

    public async Task UpdateUserAsync(User user)
    {
        var filter = Builders<User>.Filter.Eq("Id", user.Id);
        await _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }

    public async Task UserLoggedInAsync(User user)
    {
        user.LastLoggedInDate = DateTime.Now;
        var filter = Builders<User>.Filter.Eq("Id", user.Id);
        await _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<bool> CheckUsernameExists(string username)
    {
        var results = await _users.FindAsync(u => u.Username == username);
        return results.Any();
    }
}
