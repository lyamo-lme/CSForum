using CSForum.Core.Models;
using CSForum.Shared.Models.dtoModels;

namespace CSForum.WebUI.Models;

public class CreatePostViewModel
{
    public CreatePost Post { get; set; }
    public List<Tag>? Tags { get; set; } = null;
}