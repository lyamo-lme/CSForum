using Microsoft.AspNetCore.Authentication;

namespace CSForum.IdentityServer.Models;

public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
    public IEnumerable<AuthenticationScheme>? ExternalProviders { get; set;}
}