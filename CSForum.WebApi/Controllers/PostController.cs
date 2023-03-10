using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Web;
using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
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
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;

        public PostController(IUnitOfWorkRepository uofRepository, ILogger<PostController> logger,
            UserManager<User> userManager, IPostService postService)
        {
            _mapper = MapperFactory.CreateMapper<DtoMapper>();
            _uofRepository = uofRepository;
            _logger = logger;
            _userManager = userManager;
            _postService = postService;
        }

        [HttpPost, Route(""), Authorize]
        public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDto model)
        {
            try
            {
                var postEntity = _mapper.Map<Post>(model);
                postEntity.UserId = _userManager.GetId(User);
                var post = await _postService.CreatePost(postEntity);
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
                var mappedPost = _mapper.Map<Post>(model);
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
                var postResult = await _postService.FindPost(x => x.Id == postId);
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

        [HttpGet]
        [Route("tag/{tagId}")]
        public async Task<ActionResult<Post>> GetPostByTagId(int tagId)
        {
            try
            {
                var postTags = await _uofRepository.GenericRepository<PostTag>().GetAsync(
                    x => x.TagId == tagId,
                    includeProperties: "Post,Post.PostCreator");
                var posts = new List<Post?>();
                foreach (var postTag in postTags)
                {
                    posts.Add(postTag.Post);
                }

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
                    orderBy: post => post.OrderByDescending(obj => obj.DateCreate),
                    includeProperties: $"PostTags.Tag,{nameof(Post.PostCreator)}"
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