using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Services.HttpClients;
using Microsoft.AspNetCore.Mvc;
using CSForum.WebUI.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CSForum.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ApiHttpClientBase _forumClient;

    private UserManager<User> _userManager;

    public HomeController(UserManager<User> userManager, ApiHttpClientBase forumClient)
    {
        _userManager = userManager;
        _forumClient = forumClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    //created to check auth state
    [Authorize]
    public async Task<ActionResult<Post>> SecretPath()
    {
        var accToken = await HttpContext.GetTokenAsync("access_token");
        var idToken = await HttpContext.GetTokenAsync("id_token");
        var _acToken = new JwtSecurityTokenHandler().ReadJwtToken(accToken);
        var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

        _forumClient.client.SetBearerToken(accToken);
        var result = await _forumClient.GetAsync<string>("api/users/secret");

        return new Post();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return SignOut("Cookie", "oidc");
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Login(string returnUrl)
    {
        if (!User.Identity.IsAuthenticated)
            return Challenge("Cookie", "oidc");
        return Redirect(returnUrl);
    }
}