// Ignore Spelling: Favourite

namespace DexMasterLibrary.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public AuthenticationProvider AuthenticationMethods { get; set; } = new AuthenticationProvider();
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName => string.IsNullOrWhiteSpace(Username) ? $"{FirstName} {LastName}" : Username;
    public string EmailAddress { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime LastLoggedInDate { get; set; }
    public VipStatus VipStatus { get; set; } = VipStatus.None;
    public byte[] ProfilePicture { get; set; }
    public List<string> CreatedPrompts { get; set; } = new List<string>();
    public List<string> FavouritePrompts { get; set; } = new List<string>();

    public override bool Equals(object? o)
    {
        var other = o as User;
        return other?.DisplayName == DisplayName;
    }

    public override int GetHashCode() => DisplayName?.GetHashCode() ?? 0;

    public override string ToString() => DisplayName;
}

public enum VipStatus
{
    None,
    Paid,
    Admin
}

public class AuthenticationProvider
{
    public string Provider { get; set; }
    public string NameIdentifier { get; set; }
}