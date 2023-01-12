using CSForum.Core.Models;

namespace CSForum.Core.IRepositories;

public interface IPostRepository
{
    public Task<List<Post?>> GetAsync();
    public Task<Post?> GetByIdAsync(int id);
    public Task<Post> CreateAsync(Post model);
    public Task<bool> DeleteAsync(int id);
}