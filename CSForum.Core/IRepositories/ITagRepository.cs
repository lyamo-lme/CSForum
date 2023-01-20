using CSForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Core.IRepositories
{
    public interface ITagRepository
    {
        public Task<List<Tag>> GetAsync();
        public Task<Tag> FindAsync(Expression<Func<Tag, bool>> func);
        public Task<Tag> CreateAsync(Tag model);
        public Task<bool> DeleteAsync(int id);
        public Task<Tag> UpdateAsync(Tag model);
        public Task SaveChangesAsync();
    }
}
