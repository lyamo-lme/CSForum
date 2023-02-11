using System.Linq.Expressions;
using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.IRepositories.Services;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IUnitOfWorkRepository _uofRepository;
        private readonly IMapper _dtoMapper;
        private readonly IPostService _postService;

        public PostController(IUnitOfWorkRepository uofRepository, IPostService postService)
        {
            _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
            _uofRepository = uofRepository;
            _postService = postService;
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDto model)
        {
            try
            {
                var mappedPost = _dtoMapper.Map<Post>(model);
                var post = await _uofRepository.Posts.CreateAsync(mappedPost);
                await _uofRepository.SaveAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost, Route("edit")]
        public async Task<ActionResult<Post>> EditPost([FromBody] EditPostDto model)
        {
            try
            {
                var mappedPost = _dtoMapper.Map<Post>(model);
                var post =  await _uofRepository.Posts.UpdateAsync(mappedPost);
                 _uofRepository.SaveAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet, Route("id/{postId}")]
        public async Task<ActionResult<Post>> FindPost(int postId)
        {
            try
            {
                var model = await _uofRepository.Posts.FindAsync(x => x.Id == postId);
                model.PostTags =  await _uofRepository.PostTags.GetAsync(x => x.PostId == model.Id, null, null, null, "Tag");
                model.PostCreator = await _uofRepository.Users.FindAsync(x => x.Id == model.UserId);
                model.Answers = await _uofRepository.Answers.GetAsync(x=>x.PostId==model.Id,null,null,null,"AnswerCreator");
                return Ok(model);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Post>> GetPosts()
        {
            try
            {
                return Ok(await _uofRepository.Posts.GetAsync());
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [HttpGet,Route("recent/{count}")]
        public async Task<ActionResult<Post>> GetRecentPosts(int count=10)
        {
            try
            {
                return Ok(await _uofRepository.Posts.GetAsync(
                    null,
                    orderBy: post =>post.OrderByDescending(obj=>obj.DateCreate)
                    ));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpDelete, Route("delete")]
        public async Task<ActionResult<bool>> DeletePost(int postId)
        {
            try
            {
                var state = await _uofRepository.Posts.DeleteAsync(new Post()
                {
                    Id = postId
                });
                 _uofRepository.SaveAsync();
                return Ok(state);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}