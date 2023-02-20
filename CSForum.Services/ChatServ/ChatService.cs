using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using Microsoft.Extensions.Logging;

namespace CSForum.Services.ChatServ;

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
            var userChat = await GetUserChat(model.UserId, receiverId);
            if (userChat == null)
            {
                throw new Exception("chat wasn't found");
            }

            model.ChatId = userChat.ChatId;
            var message = await _uofRepository.Messages.CreateAsync(model);
            _uofRepository.SaveAsync();
            return message;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message, e);
            throw;
        }
    }

    private async Task<UsersChats?> GetUserChat(int firstId, int secondId)
    {
        try
        {
            var usersChat = await _uofRepository.UserChats.GetAsync(
                userChat => userChat.UserId == firstId);

            foreach (var userChat in usersChat)
            {
                var destinationChat = await _uofRepository.UserChats.FindAsync(
                    chat => userChat.ChatId == chat.ChatId && chat.UserId == secondId);
                if (destinationChat != null)
                {
                    return destinationChat;
                }
            }

            return null;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message, e);

            throw;
        }
    }

    public async Task<Chat> CreateChatAsync(int firstId, int secondId)
    {
        try
        {
            var userChat = await GetUserChat(firstId, secondId);

            if (userChat != null)
                throw new Exception("chat created");

            var chat = await _uofRepository.Chats.CreateAsync(new Chat());

            await _uofRepository.UserChats.CreateAsync(new UsersChats()
            {
                ChatId = chat.ChatId,
                UserId = firstId
            });

            await _uofRepository.UserChats.CreateAsync(new UsersChats()
            {
                ChatId = chat.ChatId,
                UserId = secondId
            });

            _uofRepository.SaveAsync();
            return chat;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message, e);

            throw;
        }
    }

    public async Task<List<UsersChats>> GetUserChats(int userId)
    {
        try
        {
            var userChats =
                await _uofRepository.UserChats.GetAsync(x => x.UserId == userId);
            
            var usersChats = new List<UsersChats>();
            
            foreach (var userChat in userChats)
            {
                var newChat =
                    await _uofRepository.UserChats.FindAsync(x => x.UserId != userId && x.ChatId == userChat.ChatId);
                
                newChat.Chat = await _uofRepository.Chats.FindAsync(x => x.ChatId == userChat.ChatId);
                
                newChat.Chat.Messages =
                    await _uofRepository.Messages.GetAsync(x => x.ChatId == userChat.ChatId,
                        includeProperties: "User");

                usersChats.Add(newChat);
            }

            return usersChats;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message, e);

            throw;
        }
    }
}