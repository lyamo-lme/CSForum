using AutoMapper;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models;
using CSForum.Shared.Models.dtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSForum.WebUI.Controllers;

public class PostController : Controller
{
    private IMapper mapper;
    private IPostClient _postClient;

    public PostController(IOptions<ApiSettingConfig> options, IPostClient client)
    {
        _postClient = client;
        mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [HttpGet]
    public IActionResult CreatePost()
    {
        return View("FormPost");
    }

    [HttpPost]
    public async Task<ViewResult> CreatePost(CreatePost model)
    {
        await _postClient.CreateAsync(mapper.Map<Post>(model));
        return View("Post", mapper.Map<Post>(model));
    }

    [HttpGet]
    IActionResult Post(int postId)
    {
        return View();
    }
}