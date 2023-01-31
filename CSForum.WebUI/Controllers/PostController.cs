using System.Security.Claims;
using AutoMapper;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSForum.WebUI.Controllers;

[Route("post")]
public class PostController : Controller
{
    private readonly IMapper _mapper;
    private readonly IForumClient _forumClient; 
    private readonly UserManager<User> _userManager;

    

    public PostController(IOptions<ApiSettingConfig> options, IForumClient client,
        UserManager<User> userManager)
    {
        _forumClient = client;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
        _userManager = userManager;
    }

    [HttpGet, Route("create")]
    public IActionResult CreatePost()
    {
        return View("FormPost");
    }

    [HttpPost,Route("create")]
    public async Task<ViewResult> CreatePost(CreatePostView model)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var postDto = _mapper.Map<CreatePostDto>(model);
            postDto.UserId = user.Id;
            var post = await _forumClient.PostAsync<CreatePostDto, Post>(postDto, "api/posts/create");
            return await Post(post.Id);
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