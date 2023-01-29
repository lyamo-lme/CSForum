using AutoMapper;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.WebUI.Models;
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

    [HttpPost,Route("create")]
    public async Task<ViewResult> CreatePost(CreatePostDto model)
    {
        try
        {
            model.UserId = 1;
            var post = await _forumClient.PostAsync<CreatePostDto, Post>(model, "api/posts/create");
            return View("Post", _mapper.Map<PostViewModel>(post));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet,Route("post/{postId}")]
    public  async Task<ViewResult> Post(int postId)
    {
        try
        {
            var post = await _forumClient.GetAsync<Post>($"api/posts/id/{postId}");
            return View("Post", _mapper.Map<PostViewModel>(post));
        }
        catch (Exception e)
        {
            throw;
        }
    }
}