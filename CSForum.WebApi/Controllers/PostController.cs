using System.Linq.Expressions;
using AutoMapper;
using CSForum.Core.IRepositories;
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
        private readonly ILogger<PostController> _logger;

        public PostController(IUnitOfWorkRepository uofRepository, ILogger<PostController> logger)
        {
            _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
            _uofRepository = uofRepository;
            _logger = logger;
        }

        [HttpPost, Route("")]
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
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        [HttpPut, Route("")]
        public async Task<ActionResult<Post>> EditPost([FromBody] EditPostDto model)
        {
            try
            {
                var mappedPost = _dtoMapper.Map<Post>(model);
                var post = await _uofRepository.Posts.UpdateAsync(mappedPost);
                _uofRepository.SaveAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        [HttpGet, Route("id/{postId}")]
        public async Task<ActionResult<Post>> FindPost(int postId)
        {
            try
            {
                var postResult = await _uofRepository.Posts.FindAsync(post => post.Id == postId);

                postResult.PostTags = await _uofRepository.PostTags.GetAsync(postTag => postTag.PostId == postResult.Id,
                    null, null, null, "Tag");

                postResult.PostCreator = await _uofRepository.Users.FindAsync(user => user.Id == postResult.UserId);

                postResult.Answers = await _uofRepository.Answers.GetAsync(answer => answer.PostId == postResult.Id,
                    null, null, null, "AnswerCreator");

                return Ok(postResult);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
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
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        [HttpGet, Route("recent/{count}")]
        public async Task<ActionResult<Post>> GetRecentPosts(int count = 10)
        {
            try
            {
                return Ok(await _uofRepository.Posts.GetAsync(
                    null,
                    orderBy: post => post.OrderByDescending(obj => obj.DateCreate)
                ));
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        [HttpDelete, Route("{postId}")]
        public async Task<ActionResult<bool>> DeletePost(int postId)
        {
            try
            {
                if (await _uofRepository.Posts.FindAsync(post => post.Id == postId) == null)
                {
                    return BadRequest("id failed");
                }

                var state = await _uofRepository.Posts.DeleteAsync(new Post()
                {
                    Id = postId
                });
                _uofRepository.SaveAsync();
                return Ok(state);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }
    }
}