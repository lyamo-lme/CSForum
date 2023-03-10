using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Web;
using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.Http;
using CSForum.Shared.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Posts;
using CSForum.Shared.Models.ViewModels;
using CSForum.WebUI.Models;
using CSForum.WebUI.Services.HttpClients;
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


    public PostController(ApiHttpClientBase client,
        UserManager<User> userManager)
    {
        _forumClient = client;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
        _userManager = userManager;
    }

    [HttpGet, Route("create"), Authorize]
    public IActionResult CreatePost()
    {
        return View("FormPost");
    }

    [HttpPost, Route("create")]
    [Authorize]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostView model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("FormPost", model);
            }
            var postDto = _mapper.Map<CreatePostDto>(model);
            await _forumClient.SetBearerTokenAsync();
            var post = await _forumClient.PostAsync<CreatePostDto, Post>(postDto, "api/posts");
            return RedirectToAction($"{post.Id}");
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet, Route("{postId}")]
    public async Task<ViewResult> Post(int postId)
    {
        try
        {
            var post = await _forumClient.GetAsync<Post>($"api/posts/id/{postId}");
            post.Content = HttpUtility.HtmlDecode(post.Content);
            return View("Post", _mapper.Map<PostViewModel>(post));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet, Route("recent/{take}")]
    public async Task<IActionResult> GetRecentPosts(int? take = 10)
    {
        try
        {
            var recentPosts = await _forumClient.GetAsync<List<Post>>($"api/posts/recent/{take}");
            return View("PostsView", _mapper.Map<List<PostViewModel>>(recentPosts));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet, Authorize, Route("state/{postId}")]
    public async Task<IActionResult> UpdatePostState(int? postId)
    {
        try
        {
            var post = await _forumClient.GetAsync<Post>($"api/posts/id/{postId}");
            if (post.UserId != _userManager.GetId(User))
                return Redirect($"/post/{post.Id}");
            post.Solved = !post.Solved;
            var postDto = _mapper.Map<EditPostDto>(post);
            await _forumClient.SetBearerTokenAsync();
            await _forumClient.PutAsync<EditPostDto, Post>(postDto, $"api/posts");
            return Redirect($"/post/{post.Id}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}