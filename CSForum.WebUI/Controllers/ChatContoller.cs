using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Http;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

[Route("chat")]
public class ChatController : Controller
{
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly ApiHttpClientBase _forumClient;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public ChatController(IUnitOfWorkRepository uofRepository, SignInManager<User> signInManager,
        UserManager<User> userManager, ApiHttpClientBase forumClient)
    {
        _uofRepository = uofRepository;
        _userManager = userManager;
        _forumClient = forumClient;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [Authorize, HttpGet, Route("")]
    public async Task<IActionResult> Chat()
    {
        try
        {
            await _forumClient.SetBearerTokenAsync();
            var usersChat = await _forumClient.GetAsync<List<UsersChats>>("api/chat/user");
            return View("Chat");
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [Authorize, HttpGet, Route("user")]
    public async Task<ActionResult<List<UsersChats>>> ChatsData()
    {
        try
        {
            await _forumClient.SetBearerTokenAsync();
            var usersChat = await _forumClient.GetAsync<List<UsersChats>>("api/chat/user");
            return Ok(usersChat);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateChat(int userId)
    {
        try
        {
            var signedUser = _userManager.GetUserId(User);
            var chat = await _uofRepository.Chats.CreateAsync(new Chat());
            _uofRepository.UserChats.CreateAsync(new UsersChats()
            {
                ChatId = chat.ChatId,
                UserId = userId
            });
            _uofRepository.UserChats.CreateAsync(new UsersChats()
            {
                ChatId = chat.ChatId,
                UserId = int.Parse(signedUser)
            });
            _uofRepository.SaveAsync();
            return View();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}