using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CSForum.Data.Repositories
{
    public class UowRepository:IUnitOfWorkRepository
    {
        private readonly ILogger<UowRepository> _logger;
        private readonly ForumDbContext _forumDbContext;
        private IRepository<Tag>? _tagRepository;
        private GenericRepository<Post>? _postRepository;
        private GenericRepository<User>? _userRepository;
        private GenericRepository<Answer>? _answerRepository;
        private GenericRepository<PostTag>? _postTagsRepository;
        private GenericRepository<Chat>? _chatRepository;
        private GenericRepository<Message>? _messageRepository;
        private GenericRepository<UsersChats>? _usersChatRepository;

        public IRepository<Tag> Tags => _tagRepository ?? (_tagRepository = new GenericRepository<Tag>(_forumDbContext));
        public IRepository<Post> Posts => _postRepository ?? (_postRepository = new GenericRepository<Post>(_forumDbContext));
        public IRepository<User> Users => _userRepository ?? (_userRepository = new GenericRepository<User>(_forumDbContext));
        public IRepository<Answer> Answers => _answerRepository ?? (_answerRepository = new GenericRepository<Answer>(_forumDbContext));
        public IRepository<PostTag> PostTags => _postTagsRepository ?? (_postTagsRepository = new GenericRepository<PostTag>(_forumDbContext));
        public IRepository<Chat> Chats => _chatRepository ?? (_chatRepository = new GenericRepository<Chat>(_forumDbContext));
        public IRepository<Message> Messages => _messageRepository ?? (_messageRepository = new GenericRepository<Message>(_forumDbContext));
        public IRepository<UsersChats> UserChats => _usersChatRepository ?? (_usersChatRepository = new GenericRepository<UsersChats>(_forumDbContext));
        
        // private Dictionary<string, object> _repositories;
        
        public UowRepository(ForumDbContext forumDbContext, ILogger<UowRepository> logger)
        {
            _forumDbContext = forumDbContext;
            _logger = logger;
        }
        // public GenericRepository<T> GenericRepository<T>() where T : class
        // {
        //     if (_repositories == null)
        //         _repositories = new Dictionary<string, object>();
        //     var type = typeof(T).Name;
        //     if (!_repositories.ContainsKey(type))
        //     {
        //         var repositoryType = typeof(GenericRepository<T>);
        //         var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _forumDbContext);
        //         _repositories.Add(type, repositoryInstance);
        //     }
        //     return (GenericRepository<T>)_repositories[type];
        // } 

        public Task SaveAsync()
        {
            try
            {
               return _forumDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Critical,e,e.Message);
                throw;
            }
        }
    }
}
