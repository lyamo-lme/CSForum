using System.Diagnostics;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CSForum.WebUI.Models;
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
    public ActionResult<Post> SecretPath()
    {
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