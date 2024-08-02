using System.ComponentModel.DataAnnotations;

namespace ChatApp.Client
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.
    public class UserData
    {
        public int? Id { get; set; }
        [Required] public string? Login { get; set; }
        [Required] public string? Password { get; set; }
    }
}
