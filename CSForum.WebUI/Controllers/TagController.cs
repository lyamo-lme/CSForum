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
        public ITagClient tagClient { get; set; }
        public TagController(ITagClient tagClient)
        {
            this.tagClient = tagClient;
        }
        public async Task<ActionResult> FindTag(int tagId) {

            try
            {
               return View("TagView",await tagClient.FindAsync(tagId));
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
                var tag = await tagClient.CreateAsync(model);
                return View("TagView",await tagClient.FindAsync(tag.Id));
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
