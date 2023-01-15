using CSForum.Core.Models;
using System.Linq.Expressions;

namespace CSForum.Core.IRepositories;

public interface IUserRepository
{
    public Task<List<User>> GetAsync();
    public Task<User> GetFirstByFunc(Expression<Func<User, bool>> func);
    public Task<User> CreateAsync(User model);
    public Task<bool> DeleteAsync(int id);
}