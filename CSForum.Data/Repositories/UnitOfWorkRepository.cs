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
using Microsoft.Extensions.DependencyInjection;

namespace CSForum.Data.Repositories
{
    public class UowRepository : IUnitOfWorkRepository, IDisposable
    {
        private readonly ILogger<UowRepository> _logger;
        private readonly ForumDbContext _forumDbContext;
        private readonly IRepositoryFactory _repositoryFactory;
        private Dictionary<string, object>? _repositories;

        //need to inject service collection and refactor repositories
        public UowRepository(ILogger<UowRepository> logger, IRepositoryFactory repositoryFactory,
            ForumDbContext forumDbContext)
        {
            _logger = logger;
            _repositoryFactory = repositoryFactory;
            _forumDbContext = forumDbContext;
        }

        public void Dispose()
        {
            _forumDbContext.Dispose();
        }

        public IRepository<T> GenericRepository<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = _repositoryFactory.Instance<T>(_forumDbContext);
                _repositories.Add(type, repository);
            }

            return (IRepository<T>)_repositories[type];
        }

        public Task SaveAsync()
        {
            try
            {
                return _forumDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Critical, e, e.Message);
                throw;
            }
        }
    }
}