using AutoMapper;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.Http;
using CSForum.Services.MapperConfigurations;
using CSForum.WebUI.Models;
using CSForum.WebUI.Services.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace CSForum.WebUI.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly ApiHttpClientBase _forumClient;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<User> _userManager;

    public UserController(ApiHttpClientBase forumClient, ILogger<UserController> logger, UserManager<User> userManager)
    {
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
        _forumClient = forumClient;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("info/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = _mapper.Map<UserViewModel>(
                await _forumClient.GetAsync<User>($"api/users/{id}"));
            user.MenuOption = 0;
            return View("UserView", user);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("post/{id}")]
    public async Task<IActionResult> GetUserPost(int id)
    {
        try
        {
            var user = _mapper.Map<UserViewModel>(
                await _forumClient.GetAsync<User>($"api/users/{id}"));
            user.Posts = _mapper.Map<List<PostViewModel>>(
                 await _forumClient.GetAsync<List<Post>>($"api/posts/user/{id}")
            );
            user.MenuOption = 1;
            return View("UserView", user);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }
    [HttpGet]
    [Route("answers/{id}")]
    public async Task<IActionResult> GetUserAnswers(int id)
    {
        try
        {
            var user = _mapper.Map<UserViewModel>(
                await _forumClient.GetAsync<User>($"api/users/{id}"));
            user.Answers = _mapper.Map<List<AnswerViewModel>>(
                await _forumClient.GetAsync<List<Answer>>($"api/answers/user/{id}")
            );
            user.MenuOption = 2;
            return View("UserView", user);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("id")]
    [Authorize]
    public Task<int> Id()
    {
        try
        {
            return Task.FromResult(_userManager.GetId(User));
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }
}