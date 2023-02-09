using CSForum.Core.IRepositories;
using CSForum.Core.IRepositories.Services;
using CSForum.Core.Models;

namespace CSForum.WebApi.RepositoryServices;

public class PostService:IPostService
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    public PostService(IUnitOfWorkRepository unitOfWorkRepository)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
    }

    public async Task<Post> AddPost(Post model)
    {
       var post = await _unitOfWorkRepository.Posts.CreateAsync(model);
        return post;
    }

    public Task<Post> UpdatePost(Post model)
    {
        throw new NotImplementedException();
    }
}