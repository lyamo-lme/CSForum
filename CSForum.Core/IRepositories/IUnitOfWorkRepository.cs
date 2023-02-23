using CSForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Core.IRepositories
{
    public interface IUnitOfWorkRepository
    {
        public IRepository<T> GenericRepository<T>() where T : class;
        // public IRepository<T> Repository<T>() where T : class;
        Task SaveAsync();
    }
}
