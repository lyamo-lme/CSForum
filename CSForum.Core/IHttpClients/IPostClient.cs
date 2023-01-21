using CSForum.Core.Models;

namespace CSForum.Core.IHttpClients;

public interface IPostClient
{
    public Task<Post> CreatePost(Post model);
    public Task<Post> EditPost(Post model);
    public Task<bool> DeletePost(string postId);
    public Task<List<Post>> GetPosts();
}