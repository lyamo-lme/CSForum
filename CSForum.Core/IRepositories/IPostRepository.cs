using System.Linq.Expressions;
using CSForum.Core.Models;

namespace CSForum.Core.IRepositories;

public interface IPostRepository
{
    public Task<List<Post>> GetAsync();
    public Task<Post> FindAsync(Expression<Func<Post, bool>> func);
    public Task<Post> CreateAsync(Post model);
    public Task<bool> DeleteAsync(int id);
    public Task<Post> UpdateAsync(Post model);
    public Task SaveChangesAsync();
}