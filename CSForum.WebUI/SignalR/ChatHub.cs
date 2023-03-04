using System.Net;
using AutoMapper;
using CSForum.Core;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.MapperConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CSForum.WebUI.SignalR
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatHub> _logger;
        private readonly IChatService _chatService;
        private readonly IUnitOfWorkRepository _uofRepository;

        public ChatHub(UserManager<User> userManager, IUnitOfWorkRepository uofRepository, ILogger<ChatHub> logger,
            IChatService chatService)
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

                var messageEntity = await _chatService.AddMessageAsync(
                    new Message(int.Parse(userId),
                        WebUtility.HtmlEncode(message)
                    ), receiverId);

                await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", messageEntity);
                await Clients.Caller.SendAsync("ReceiveMessage", messageEntity);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                return base.OnConnectedAsync();
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