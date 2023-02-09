using System.Security.Claims;
using AutoMapper;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.HttpClients;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Posts;
using CSForum.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSForum.WebUI.Controllers;

[Route("post")]
public class PostController : Controller
{
    private readonly IMapper _mapper;
    private readonly ApiHttpClientBase _forumClient; 
    private readonly UserManager<User> _userManager;

    

    public PostController(IOptions<ApiSettingConfig> options, ApiHttpClientBase client,
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
    [Authorize]
    public async Task<IActionResult> CreatePost(CreatePostView model)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var postDto = _mapper.Map<CreatePostDto>(model);
            postDto.UserId = user.Id;
            var post = await _forumClient.PostAsync<CreatePostDto, Post>(postDto, "api/posts/create");
            return RedirectToAction($"{post.Id}");
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet,Route("{postId}")]
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

    public async Task<IActionResult> GetRecentPosts(int? take=null)
    {
        try
        {
            var recentPosts =  await _forumClient.GetAsync<List<Post>>($"api/posts/recent/{take}");
            return View("PostsView", _mapper.Map<List<PostViewModel>>(recentPosts));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}