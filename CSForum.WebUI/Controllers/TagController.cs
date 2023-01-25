using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
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
        public async Task<ActionResult<Tag>> FindTag(int tagId) {

            try
            {
               return View("TagView",await tagClient.FindAsync(tagId));
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
