using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController : Controller
    {
        
        private readonly ITagRepository TagRepository;

        public TagController(ITagRepository postRepository)
        {
            TagRepository = postRepository;
        }
        [HttpPost]
        public async Task<ActionResult<Tag>> CreatePost([FromBody] Tag model)
        {
            try
            {
                var post = await TagRepository.CreateAsync(model);
                await TagRepository.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> EditPost([FromBody] Tag model)
        {
            try
            {
                var post = await TagRepository.UpdateAsync(model);
                await TagRepository.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        [HttpGet]
        public async Task<ActionResult<Tag>> GetPost(int postId)
        {
            try
            {
                return Ok(await TagRepository.FindAsync(x => x.Id == postId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeletePost(int postId)
        {
            try
            {
                var state = await TagRepository.DeleteAsync(postId);
                return Ok(state);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
