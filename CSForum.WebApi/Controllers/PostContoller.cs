
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostContoller : Controller
    {
        private readonly IPostRepository PostRepository;
        // private readonly IMapper mapper;
        public PostContoller(IPostRepository postRepository)
        {
            PostRepository = postRepository;
        }
        public async Task<ActionResult<Post>> CreatePost(Post model)
        {
            try
            {
                return Ok(await PostRepository.CreateAsync(model));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        public async Task<ActionResult<Post>> EditPost(Post model)
        {
            try
            {
                return Ok(await PostRepository.UpdateAsync(model));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}