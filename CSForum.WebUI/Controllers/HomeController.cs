using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CSForum.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace CSForum.WebUI.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
    }
    
    public IActionResult Index()
    {
        return View();
    }
    [Authorize]
    public async Task<ActionResult<Post>> SecretPath()
    {
        var accToken = await HttpContext.GetTokenAsync("access_token");
        var idToken = await HttpContext.GetTokenAsync("id_token");
        var refresh = await HttpContext.GetTokenAsync("refresh_token");
        var _acToken = new JwtSecurityTokenHandler().ReadJwtToken(accToken);
        var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
        
        return new  Post();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}