using AutoMapper;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.Http;
using CSForum.Shared.Models.ViewModels;
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
    [Route("{id}")]
    [Route("{id}/{additionalInfo}")]
    public async Task<IActionResult> GetUserById(int id, string? additionalInfo = null)
    {
        try
        {
            var addPath = $"/{(additionalInfo == null ? "null":additionalInfo)}";
            var user = _mapper.Map<UserViewModel>(
                await _forumClient.GetAsync<User>($"api/users/{id}{addPath}"));
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