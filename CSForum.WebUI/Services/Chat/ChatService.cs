using CSForum.Core;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;

namespace CSForum.WebUI.Services.Chat;

public class ChatService : IChatService
{
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly ILogger<ChatService> _logger;

    public ChatService(IUnitOfWorkRepository uofRepository, ILogger<ChatService> logger)
    {
        _uofRepository = uofRepository;
        _logger = logger;
    }

    public async Task<Message> AddMessageAsync(Message model, int receiverId)
    {
        try
        {
            var usersChat = await _uofRepository.UserChats.GetAsync(
                userChat => userChat.UserId == model.UserId);

            foreach (var userChat in usersChat)
            {
                var destinationChat = await _uofRepository.UserChats.FindAsync(
                    chat => userChat.ChatId == chat.ChatId && chat.UserId == receiverId);
                if (destinationChat != null)
                {
                    model.ChatId = destinationChat.ChatId;
                    return await _uofRepository.Messages.CreateAsync(model);
                }
            }
            throw new Exception("chat wasn't found");
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message, e);
            throw;
        }
    }
}