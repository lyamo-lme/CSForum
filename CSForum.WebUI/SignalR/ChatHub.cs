using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace CSForum.WebUI.SignalR
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkRepository _uofRepository;

        public ChatHub(UserManager<User> userManager, IUnitOfWorkRepository uofRepository)
        {
            _mapper = MapperFactory.CreateMapper<DtoMapper>();
            _userManager = userManager;
            _uofRepository = uofRepository;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            try
            {
                await Clients.User(receiverId).SendAsync("ReceiveMessage", message);
                await Clients.Caller.SendAsync("ReceiveMessage", message);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            
            return  base.OnDisconnectedAsync(exception);
        }

    }
}