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
            var message = await _uofRepository.GenericRepository<Message>().CreateAsync(model);
            await _uofRepository.SaveAsync();
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
            var usersChat = await _uofRepository.GenericRepository<UsersChats>().GetAsync(
                userChat => userChat.UserId == firstId);

            foreach (var userChat in usersChat)
            {
                var destinationChat = await _uofRepository.GenericRepository<UsersChats>().FindAsync(
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

    public async Task<Chat> CreateChatAsync(int receiverId, Message message)
    {
        try
        {
            var userChat = await GetUserChat(receiverId, message.UserId);

            if (userChat != null)
                throw new Exception("chat created");

            var chat = await _uofRepository.GenericRepository<Chat>().CreateAsync(new Chat());
            await _uofRepository.SaveAsync();
            await _uofRepository.GenericRepository<UsersChats>().CreateAsync(new UsersChats()
            {
                ChatId = chat.ChatId,
                UserId = receiverId
            });

            await _uofRepository.GenericRepository<UsersChats>().CreateAsync(new UsersChats()
            {
                ChatId = chat.ChatId,
                UserId = message.UserId
            });

            message.ChatId = chat.ChatId;

            await _uofRepository.GenericRepository<Message>().CreateAsync(message);
            await _uofRepository.SaveAsync();
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
                (await _uofRepository.GenericRepository<UsersChats>().GetAsync(x => x.UserId == userId)).ToList();

            var usersChats = new List<UsersChats>();

            foreach (var userChat in userChats)
            {
                var newChat =
                    await _uofRepository.GenericRepository<UsersChats>()
                        .FindAsync(x => x.UserId != userId && x.ChatId == userChat.ChatId);

                newChat.User = await _uofRepository.GenericRepository<User>().FindAsync(x => x.Id == newChat.UserId);

                newChat.Chat = await _uofRepository.GenericRepository<Chat>()
                    .FindAsync(x => x.ChatId == userChat.ChatId);

                newChat.Chat.Messages =
                    (await _uofRepository.GenericRepository<Message>().GetAsync(x => x.ChatId == userChat.ChatId,
                        includeProperties: "User")).ToList();

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