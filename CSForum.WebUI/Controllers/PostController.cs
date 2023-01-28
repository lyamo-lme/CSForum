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
    private readonly IMapper _mapper;
    private readonly IForumClient _forumClient;

    public PostController(IOptions<ApiSettingConfig> options, IForumClient client)
    {
        _forumClient = client;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [HttpGet]
    public IActionResult CreatePost()
    {
        return View("FormPost");
    }

    [HttpPost]
    public async Task<ViewResult> CreatePost(CreatePostDto model)
    {
        try
        {
            var post = await _forumClient.PostAsync<CreatePostDto, Post>(model, "/api/post/create");
            return View("Post", post);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet]
    public  async Task<ViewResult> Post(int postId)
    {
        try
        {
            var post = await _forumClient.GetAsync<Post>($"/api/posts/id/{postId}");
            return View("Post", post);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}