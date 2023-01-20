using AutoMapper;
using CSForum.Core.Models;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
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
        return View("Post", mapper.Map<Post>(model));
    }

    [HttpGet]
    IActionResult Post(int postId)
    {
        return View();
    }
}