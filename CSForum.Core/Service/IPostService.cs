using System.Linq.Expressions;
using CSForum.Core.Models;

namespace CSForum.Core.Service;

public interface IPostService
{
    public Task<Post> CreatePost(Post model);
    public Task<Post> FindPost(Expression<Func<Post, bool>> expression);

}