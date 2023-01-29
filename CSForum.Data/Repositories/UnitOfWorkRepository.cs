﻿using CSForum.Core.IRepositories;
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
    public class UOWRepository:IUnitOfWorkRepository
    {
        private readonly ForumDbContext forumDbContext;
        private GenericRepository<Tag>? _tagRepository;
        private GenericRepository<Post>? _postRepository;
        private GenericRepository<User>? _userRepository;
        private GenericRepository<Answer>? _answerRepository;
        private GenericRepository<PostTag>? _postTagsRepository;
        
        public IRepository<Tag> Tags => _tagRepository ?? (_tagRepository = new GenericRepository<Tag>(forumDbContext));
        public IRepository<Post> Posts => _postRepository ?? (_postRepository = new GenericRepository<Post>(forumDbContext));
        public IRepository<User> Users => _userRepository ?? (_userRepository = new GenericRepository<User>(forumDbContext));
        public IRepository<Answer> Answers => _answerRepository ?? (_answerRepository = new GenericRepository<Answer>(forumDbContext));
        public IRepository<PostTag> PostTags => _postTagsRepository ?? (_postTagsRepository = new GenericRepository<PostTag>(forumDbContext));

        public UOWRepository(ForumDbContext forumDbContext)
        {
            this.forumDbContext = forumDbContext;
        }
        public Task SaveAsync() => forumDbContext.SaveChangesAsync();

    }
}