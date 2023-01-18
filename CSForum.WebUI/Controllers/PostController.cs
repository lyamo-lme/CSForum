using CSForum.Shared.dtoModels;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class PostController:Controller
{
    public PostController()

    {
        
    }

    [HttpGet]
    public IActionResult CreatePost()
    {
        return View("FormPost");
    }
    
    [HttpPost]
    public IActionResult CreatePost(CreatePost model)
    {     
        return View();
    }
}