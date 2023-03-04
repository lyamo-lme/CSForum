using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Web;
using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IUnitOfWorkRepository _uofRepository;
        private readonly IMapper _dtoMapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PostController> _logger;

        public PostController(IUnitOfWorkRepository uofRepository, ILogger<PostController> logger,
            UserManager<User> userManager)
        {
            _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
            _uofRepository = uofRepository;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost, Route("")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDto model)
        {
            try
            {
                model.Content = HttpUtility.HtmlEncode(model.Content);
                var mappedPost = _dtoMapper.Map<Post>(model);
                var post = await _uofRepository.GenericRepository<Post>().CreateAsync(mappedPost);
                await _uofRepository.SaveAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                return BadRequest();
            }
        }

        [HttpPut, Authorize, Route("")]
        public async Task<ActionResult<Post>> EditPost([FromBody] EditPostDto model)
        {
            try
            {
                var repository = _uofRepository.GenericRepository<Post>();
                var mappedPost = _dtoMapper.Map<Post>(model);
                var post = await repository.UpdateAsync(mappedPost);
                await _uofRepository.SaveAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Route("id/{postId}")]
        public async Task<ActionResult<Post>> FindPost(int postId)
        {
            try
            {
                var postResult = await _uofRepository.GenericRepository<Post>().FindAsync(post => post.Id == postId);

                postResult.PostTags = (await _uofRepository.GenericRepository<PostTag>().GetAsync(
                    postTag => postTag.PostId == postResult.Id,
                    null, null, null, "Tag")).ToList();

                postResult.PostCreator = await _uofRepository.GenericRepository<User>()
                    .FindAsync(user => user.Id == postResult.UserId);

                postResult.Answers = (await _uofRepository.GenericRepository<Answer>().GetAsync(
                    answer => answer.PostId == postResult.Id,
                    includeProperties: "AnswerCreator")).ToList();

                return Ok(postResult);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult<Post>> GetPosts(int? skip = null, int? take = null)
        {
            try
            {
                return Ok(await _uofRepository.GenericRepository<Post>().GetAsync(
                    skip: skip,
                    take: take
                ));
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<Post>> GetPostByUserId(int userId)
        {
            try
            {
                var posts = await _uofRepository.GenericRepository<Post>().GetAsync(x => x.UserId == userId);
                return Ok(posts);
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
                return Ok(await _uofRepository.GenericRepository<Post>().GetAsync(
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
                if (await _uofRepository.GenericRepository<Post>().FindAsync(post => post.Id == postId) == null)
                {
                    return BadRequest("id failed");
                }

                var state = await _uofRepository.GenericRepository<Post>().DeleteAsync(new Post()
                {
                    Id = postId
                });
                await _uofRepository.SaveAsync();
                return Ok(state);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                return BadRequest();
            }
        }
    }
}