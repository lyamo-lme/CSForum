using CSForum.Core.Models;
using System.Linq.Expressions;

namespace CSForum.Core.IRepositories;

public interface IUserRepository
{
    public Task<List<User>> GetAsync();
    public Task<User> FindAsync(Expression<Func<User, bool>> func);
    public Task<User> CreateAsync(User model);
    public Task<bool> DeleteAsync(string id);
    public Task<User> UpdateAsync(User model);
    public Task SaveChangesAsync();

}