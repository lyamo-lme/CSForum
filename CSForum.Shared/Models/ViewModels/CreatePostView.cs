using CSForum.Shared.Models.dtoModels.Tags;

namespace CSForum.WebUI.Models;

public class CreatePostView
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<TagIdInPostDto>? PostTags { get; set; }
}