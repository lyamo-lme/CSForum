using CSForum.Core.Models;

namespace CSForum.Core.IRepositories.Services;

public interface IPostService
{
    public Task<Post> AddPost(Post model);
    public Task<Post> UpdatePost(Post model);
}