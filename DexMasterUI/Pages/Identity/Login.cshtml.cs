using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace DexMasterUI.Pages.Identity
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private IUserData _userData;

        public LoginModel(IServiceProvider serviceProvider)
        {
            _userData = serviceProvider.GetService<IUserData>();
        }

        public IActionResult OnGet(string provider, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(provider))
            {
                // If no provider is specified, default to Google.
                provider = "Google";
            }

            // Request a redirect to the external login provider.
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("./Login",
                pageHandler: "Callback",
                values: new { returnUrl }),
            };
            return new ChallengeResult(provider, authenticationProperties);
        }

        public async Task<IActionResult> OnGetCallback(string returnUrl = null, string remoteError = null)
        {
            // Get the information about the user from the external login provider
            ClaimsIdentity authenticatedUser = User.Identities.FirstOrDefault();

            if (authenticatedUser is { IsAuthenticated: true })
            {
                User user = await _userData.GetUserFromAuthentication(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                if (string.IsNullOrEmpty(user.Id))
                {
                    var newUser = new User()
                    {
                        AuthenticationMethod = new AuthenticationProvider()
                        {
                            Provider = authenticatedUser.AuthenticationType!,
                            NameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier)!
                        },
                        EmailAddress = User.FindFirstValue(ClaimTypes.Email)!
                    };

                    await _userData.CreateUserAsync(newUser);
                }
                else
                {
                    user.LastLoggedInDate = DateTime.Now;
                    await _userData.UpdateUserAsync(user);
                }
                
                if (user.IsAdmin)
                {
                    authenticatedUser.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = Request.Host.Value
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(authenticatedUser),
                    authProperties);

                return LocalRedirect("/");
            }

            return LocalRedirect("/");
        }
    }
}
