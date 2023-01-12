using CSForum.Core.Models;

namespace CSForum.Core.IRepositories;

public interface IUserRepository
{
    public Task<List<User>> GetAsync();
    public Task<User> GetByIdAsync(int id);
    public Task<User> CreateAsync(User model);
    public Task<bool> DeleteAsync(int id);
}