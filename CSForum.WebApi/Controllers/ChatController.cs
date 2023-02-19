using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : Controller
{
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IChatService _chatService;

    public ChatController(IUnitOfWorkRepository uofRepository, UserManager<User> userManager, IChatService chatService)
    {
        _uofRepository = uofRepository;
        _userManager = userManager;
        _chatService = chatService;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [HttpGet, Route("user"), Authorize]
    public async Task<ActionResult<List<UsersChats>>> GetUsersChats()
    {
        try
        {
            var signedUser = _userManager.GetUserId(User);
            var userChats =
                await _uofRepository.UserChats.GetAsync(x => x.UserId == int.Parse(signedUser),
                    includeProperties: "User");

            foreach (var userChat in userChats)
            {
                userChat.Chat = await _uofRepository.Chats.FindAsync(x => x.ChatId == userChat.ChatId);
                userChat.Chat.Messages =
                    await _uofRepository.Messages.GetAsync(x => x.ChatId == userChat.ChatId, includeProperties: "User");
            }

            return Ok(userChats);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}