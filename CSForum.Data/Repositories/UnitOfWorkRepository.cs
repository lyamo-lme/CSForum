using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Data.Repositories
{
    public class UowRepository:IUnitOfWorkRepository
    {
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

        public UowRepository(ForumDbContext forumDbContext)
        {
            _forumDbContext = forumDbContext;
        }

        public Task SaveAsync() => _forumDbContext.SaveChangesAsync();

    }
}
