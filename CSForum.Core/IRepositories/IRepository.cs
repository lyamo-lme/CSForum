using CSForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Core.IRepositories
{
    public interface IRepository<T> where T:class
    {
        public Task<IEnumerable<T>> GetAsync();
        public Task<IEnumerable<T>> GetByFuncExpAsync(Func<T, bool> func);
        public Task<T> FindAsync(Expression<Func<T, bool>> func);
        public Task<T> CreateAsync(T model);
        public Task<bool> DeleteAsync(int id);
        public Task<T> UpdateAsync(T model);
        public Task SaveChangesAsync();
    }
}
