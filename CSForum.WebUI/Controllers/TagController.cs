using AutoMapper;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Http;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Tags;
using CSForum.WebUI.Models;
using CSForum.WebUI.Services.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers
{
    [Route("tag")]
    public class TagController : Controller
    {
        private readonly ApiHttpClientBase _forumClient;
        private readonly IMapper _mapper;
        public TagController(ApiHttpClientBase tagClient)
        {
            _forumClient = tagClient;
            _mapper = MapperFactory.CreateMapper<DtoMapper>();
        }
        [HttpGet]
        [Route("{tagId}")]
        public async Task<ActionResult> FindTag(int tagId)
        {

            try
            {
                var tag = await _forumClient.GetAsync<Tag>($"api/tags/tagId/{tagId}");
                tag.Posts = await _forumClient.GetAsync<List<Post>>($"api/posts/tag/{tagId}");
                return View("TagView",_mapper.Map<TagViewModel>(tag));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Tag()
        {

            try
            {
                return View("TagsView");
            } 
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateTag(CreateTagDto model)
        {
            try
            {
                var tag = await _forumClient.PostAsync<CreateTagDto, Tag>(model, "api/tags/create");
                return View("TagView", new TagViewModel());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
