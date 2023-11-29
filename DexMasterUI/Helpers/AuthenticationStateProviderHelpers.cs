namespace DexMasterUI.Helpers;

public static class AuthenticationStateProviderHelpers
{
    /// <summary>
    /// Get the User from the AuthenticationStateProvider
    /// </summary>
    public static async Task<User> GetUserFromAuthAsync(this AuthenticationStateProvider provider, IUserData userData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        var objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
        if (objectId is null)
        {
            return null;
        }
        return await userData.GetUserFromAuthentication(objectId);
    }
}