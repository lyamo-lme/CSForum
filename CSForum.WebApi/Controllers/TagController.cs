using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController : Controller
    {
        
        private readonly IRepository<Tag> TagRepository;

        public TagController(IRepository<Tag> postRepository)
        {
            TagRepository = postRepository;
        }
        [HttpPost, Route("create")]
        public async Task<ActionResult<Tag>> CreateTag([FromBody] Tag model)
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

        [HttpPost, Route("edit")]
        public async Task<ActionResult<Tag>> EditTag([FromBody] Tag model)
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
        [HttpGet, Route("tagId/{tagId}")]
        public async Task<ActionResult<Tag>> FindTag(int tagId)
        {
            try
            {
                return Ok(await TagRepository.FindAsync(x => x.Id == tagId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        [HttpGet, Route("{name}")]
        public async Task<ActionResult<Tag>> FindByName(string name)
        {
            try
            {
                return Ok(await TagRepository.GetByFuncExpAsync(x=>x.Name.Contains(name)));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpDelete, Route("delete")]
        public async Task<ActionResult<bool>> DeleteTag(int tagId)
        {
            try
            {
                var state = await TagRepository.DeleteAsync(tagId);
                await TagRepository.SaveChangesAsync();

                return Ok(state);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<ActionResult<Post>> GetPosts()
        {
            try
            {
                return Ok(await TagRepository.GetAsync());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
