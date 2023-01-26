using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostContoller : Controller
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IMapper _dtoMapper;

        public PostContoller(IRepository<Post> postRepository)
        {
            _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
            _postRepository = postRepository;
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDto model)
        {
            try
            {
                var mappedPost = _dtoMapper.Map<Post>(model);
                /* need to set user id and after add tags*/

                var post = await _postRepository.CreateAsync(mappedPost);
                await _postRepository.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpPost, Route("edit")]
        public async Task<ActionResult<Post>> EditPost([FromBody] EditPostDto model)
        {
            try
            {
                var mappedPost = _dtoMapper.Map<Post>(model);
                var post = await _postRepository.UpdateAsync(mappedPost);
                await _postRepository.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpGet, Route("id/{postId}")]
        public async Task<ActionResult<Post>> FindPost(int postId)
        {
            try
            {
                return Ok(await _postRepository.FindAsync(x => x.Id == postId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpGet]
        public async Task<ActionResult<Post>> GetPosts()
        {
            try
            {
                return Ok(await _postRepository.GetAsync());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpDelete, Route("delete")]
        public async Task<ActionResult<bool>> DeletePost(int postId)
        {
            try
            {
                var state = await _postRepository.DeleteAsync(postId);
                await _postRepository.SaveChangesAsync();
                return Ok(state);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}