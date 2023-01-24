using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostContoller : Controller
    {
        private readonly IRepository<Post> PostRepository;

        // private readonly IMapper mapper;
        public PostContoller(IRepository<Post> postRepository)
        {
            PostRepository = postRepository;
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post model)
        {
            try
            {
                var post = await PostRepository.CreateAsync(model);
                await PostRepository.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpPost, Route("edit")]
        public async Task<ActionResult<Post>> EditPost([FromBody] Post model)
        {
            try
            {
                var post = await PostRepository.UpdateAsync(model);
                await PostRepository.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpGet, Route("id")]
        public async Task<ActionResult<Post>> FindPost(int postId)
        {
            try
            {
                return Ok(await PostRepository.FindAsync(x => x.Id == postId));
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
                return Ok(await PostRepository.GetAsync());
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
                var state = await PostRepository.DeleteAsync(postId);
                await PostRepository.SaveChangesAsync();
                return Ok(state);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}