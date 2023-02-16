using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class ChatController:Controller
{
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    public ChatController(IUnitOfWorkRepository uofRepository, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _uofRepository = uofRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [Authorize, Route("")]
    public async Task<IActionResult> Chat()
    {
        try
        {
            var signedUser = _userManager.GetUserId(User);
            var userChat = await _uofRepository.UserChats.GetAsync(x => x.UserId == int.Parse(signedUser), includeProperties: "User");
            return View("Chat", _mapper.Map<List<UsersChatViewModel>>(userChat));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [Authorize]
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