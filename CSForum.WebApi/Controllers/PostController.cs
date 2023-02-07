using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
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

        public PostController(IUnitOfWorkRepository uofRepository)
        {
            _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
            _uofRepository = uofRepository;
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
                model.PostCreator = await _uofRepository.Users.FindAsync(x => x.Id == model.UserId);
                model.Answers = await _uofRepository.Answers.GetAsync(x=>x.PostId==model.Id,null,"AnswerCreator");
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