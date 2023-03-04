using CSForum.Core.Models;
using CSForum.Shared.Models.dtoModels.Tags;

namespace CSForum.Shared.Models.dtoModels.Posts;

public class CreatePostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<TagIdInPostDto>? PostTags { get; set; }
}