using AutoMapper;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.dtoModels;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class PostController:Controller
{
    private IMapper mapper;
    public PostController()
    {
        mapper = MapperFactory.CreateMapper<DtoMapper>();
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