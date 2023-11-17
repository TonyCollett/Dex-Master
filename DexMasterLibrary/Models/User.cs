namespace DexMasterLibrary.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public AuthenticationProvider AuthenticationMethod { get; set; } = new ();
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime LastLoggedInDate { get; set; }
    public IEnumerable<UserPokemon> UserPokemon { get; set; } = new List<UserPokemon>();

    public byte[] ProfilePicture { get; set; }
    public bool IsAdmin { get; set; }
}

public class AuthenticationProvider
{
    public string Provider { get; set; }
    public string NameIdentifier { get; set; }
}