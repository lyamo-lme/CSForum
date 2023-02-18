using AutoMapper;
using CSForum.Core;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;
using CSForum.WebUI.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace CSForum.WebUI.SignalR
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkRepository _uofRepository;
        private readonly ILogger<ChatHub> _logger;
        private readonly IChatService _chatService;

        public ChatHub(UserManager<User> userManager, IUnitOfWorkRepository uofRepository, ILogger<ChatHub> logger, IChatService chatService)
        {
            _mapper = MapperFactory.CreateMapper<DtoMapper>();
            _userManager = userManager;
            _uofRepository = uofRepository;
            _logger = logger;
            _chatService = chatService;
        }
        
        [Authorize]
        public async Task SendMessage(int receiverId, string message)
        {
            try
            {
                var userId = _userManager.GetUserId(Context.User);
                
                await _chatService.AddMessageAsync(
                    new Message(int.Parse(userId),message), receiverId);
                
                await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", message);
                await Clients.Caller.SendAsync("ReceiveMessage", message);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        public override  Task OnConnectedAsync()
        {
            try
            {
               return  base.OnConnectedAsync();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}