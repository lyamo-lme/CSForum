using CSForum.Core.Models;

namespace CSForum.Core.Service;

public interface IChatService
{
    public Task<Message> AddMessageAsync(Message model, int receiverId);
    public Task<Chat> CreateChatAsync(int receiverId,Message message);
    public Task<List<UsersChats>> GetUserChats(int userId);
}