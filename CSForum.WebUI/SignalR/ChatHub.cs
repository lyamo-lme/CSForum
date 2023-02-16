using Microsoft.AspNetCore.SignalR;

namespace CSForum.WebUI.SignalR
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var context = Context.User.Claims;
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
