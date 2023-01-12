using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CSForum.WebUI.Models;

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
}