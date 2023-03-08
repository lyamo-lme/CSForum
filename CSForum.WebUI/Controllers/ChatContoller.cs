using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.Http;
using CSForum.Shared.Models.dtoModels.Chat;
using CSForum.Shared.Models.ViewModels;
using CSForum.WebUI.Views.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

[Route("web/chat")]
public class ChatController : Controller
{
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly ApiHttpClientBase _forumClient;
    private readonly UserManager<User> _userManager;
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;

    public ChatController(IUnitOfWorkRepository uofRepository, SignInManager<User> signInManager,
        UserManager<User> userManager, ApiHttpClientBase forumClient, IChatService chatService)
    {
        _uofRepository = uofRepository;
        _userManager = userManager;
        _forumClient = forumClient;
        _chatService = chatService;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [Authorize, HttpGet, Route("")]
    public Task<ViewResult> Chat()
    {
        try
        {
            return Task.FromResult(View("Chat"));
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
    public async Task<ActionResult> CreateChat(CreateChatDto dto)
    {
        try
        {
            var signedUser = _userManager.GetId(User);
            var redirect = Url.Action(nameof(Chat), "Chat");
            await _chatService.CreateChatAsync(dto.UserId, new Message()
            {
                UserId = signedUser,
                Content = dto.Content
            });
            return Redirect(redirect);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}