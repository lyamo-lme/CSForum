using CSForum.Core.Models;

namespace CSForum.Core.Service;

public interface IChatService
{
    public Task<Message> AddMessageAsync(Message model, int receiverId);
}