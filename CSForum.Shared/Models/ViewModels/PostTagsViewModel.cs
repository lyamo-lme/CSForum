using CSForum.Core.Models;
using CSForum.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Shared.Models.ViewModels
{
    public class PostTagsViewModel
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
        public PostViewModel? Post { get; set; }
        public TagViewModel? Tag { get; set; }
    }
}
