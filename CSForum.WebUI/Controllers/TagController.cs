using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Tag;
using CSForum.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers
{
    public class TagController:Controller
    {
        private readonly IForumClient _forumClient;
        public TagController(IForumClient tagClient)
        {
            this._forumClient = tagClient;
        }
        public async Task<ActionResult> FindTag(int tagId) {

            try
            {
               return View("TagView",await _forumClient.GetAsync<Tag>($"api/tags/tagId/{tagId}"));
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<ActionResult> CreateTag(CreateTagDto model)
        {
            try
            {
                var tag = await _forumClient.PostAsync<CreateTagDto, Tag>(model, "api/tags/create");
                return View("TagView",tag);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
