using System.Linq.Expressions;
using System.Web;
using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;

namespace CSForum.Services.EntityService;

public class PostService:IPostService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWorkRepository _uofRepository;

    public PostService(IUnitOfWorkRepository uofRepository)
    {
        _uofRepository = uofRepository;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }
    public async Task<Post> CreatePost(Post model)
    {
        model.Content = HttpUtility.HtmlEncode(model.Content);
        var mappedPost = _mapper.Map<Post>(model);
        
        var post = await _uofRepository.GenericRepository<Post>().CreateAsync(mappedPost);
        
        var user = await _uofRepository.GenericRepository<User>().FindAsync(x=>x.Id==model.UserId);
        
        await _uofRepository.GenericRepository<User>().UpdateAsync(new User()
        {
            Id = user.Id,
            RatingScores = user.RatingScores + 1
        });
        
        await _uofRepository.SaveAsync();
        return post;
    }

    public async Task<Post> FindPost(Expression<Func<Post, bool>> expression)
    {
        try
        {
            var postResult = await _uofRepository.GenericRepository<Post>().FindAsync(expression);
            if (postResult == null)
            {
                throw new NullReferenceException();
            }

            postResult.PostTags = (await _uofRepository.GenericRepository<PostTag>().GetAsync(
                postTag => postTag.PostId == postResult.Id,
                null, null, null, "Tag")).ToList();

            postResult.PostCreator = await _uofRepository.GenericRepository<User>()
                .FindAsync(user => user.Id == postResult.UserId);

            postResult.Answers = (await _uofRepository.GenericRepository<Answer>().GetAsync(
                answer => answer.PostId == postResult.Id,
                includeProperties: "AnswerCreator")).ToList();
            return postResult;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}