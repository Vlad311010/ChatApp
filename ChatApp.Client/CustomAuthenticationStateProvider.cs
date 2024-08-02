using ChatApp.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;


namespace ChatApp.Client
{

    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static AuthenticationState? currentAuthenticationState = null;

        public ClaimsPrincipal AnonymousUser => new(new ClaimsIdentity(Array.Empty<Claim>()));


        private ClaimsPrincipal FakedUser
        {
            get
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "Ruf"),
                    new Claim(ClaimTypes.NameIdentifier, "Ruf"),
                    new Claim("UserId", "1")
                };

                var identity = new ClaimsIdentity(claims, "ClientSide");
                return new ClaimsPrincipal(identity);
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return await (
                currentAuthenticationState == null
                ? Task.FromResult(new AuthenticationState(AnonymousUser))
                : Task.FromResult(currentAuthenticationState)
            );
        }

        public void LoginUser(string login)
        {
            Console.WriteLine("login attempt: " + login);
            // User? user = await usersRepository.GetByLogin(login);
            User? user = new User(login, "1");
            if (user == null)
            {
                Console.WriteLine("Error: Invalid login attempt.");
                // errorMessage = "Error: Invalid login attempt.";
                return;
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.NameIdentifier, login),
                new Claim("UserId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "ClientSide");

            var result = new AuthenticationState(new ClaimsPrincipal(claimsIdentity));
            currentAuthenticationState = result;
            NotifyAuthenticationStateChanged(Task.FromResult(result));
        }


        public void SignOut()
        {
            var result = Task.FromResult(new AuthenticationState(AnonymousUser));
            currentAuthenticationState = null!;
            NotifyAuthenticationStateChanged(result);
        }
    }
}