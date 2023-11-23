﻿namespace DexMasterLibrary.DataAccess;

public class MongoUserData : IUserData
{
    private readonly IMongoCollection<User> _users;

    public MongoUserData(IDbConnection db)
    {
        _users = db.UserCollection;
    }

    public async Task<List<User>> GetAllUsersAsync() 
        => await _users.Find(_ => true).ToListAsync();

    public async Task<User> GetUserByIdAsync(string id) 
        => await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

    public async Task<User> GetUserByUsernameAsync(string username) 
        => await _users.Find(u => u.Username == username).FirstOrDefaultAsync();

    public async Task<User> GetUserFromAuthentication(string objectId) 
        => await _users.Find(u => u.AuthenticationMethod.NameIdentifier == objectId).FirstOrDefaultAsync();

    public async Task CreateUserAsync(User user)
        => await _users.InsertOneAsync(user);
    

    public async Task CreateMultipleUsersAsync(IEnumerable<User> users)
        => await _users.InsertManyAsync(users);

    public async Task UpdateUserAsync(User user)
    {
        var filter = Builders<User>.Filter.Eq("Id", user.Id);
        await _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }
    
    public async Task UpdateUsernameByIdAsync(string userId, string username)
    {
        User user = await GetUserByIdAsync(userId);
        user.Username = username;
        var filter = Builders<User>.Filter.Eq("Id", userId);
        await _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }

    public async Task UserLoggedInAsync(User user)
    {
        user.LastLoggedInDate = DateTime.Now;
        var filter = Builders<User>.Filter.Eq("Id", user.Id);
        await _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<bool> CheckUsernameExists(string username) 
        => await _users.Find(u => u.Username == username).AnyAsync();
}
