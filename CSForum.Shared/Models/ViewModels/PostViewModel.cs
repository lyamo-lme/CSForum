using CSForum.Core.Models;
using CSForum.Shared.Models.ViewModels;

namespace CSForum.WebUI.Models;

public class PostViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public DateTime DateCreate { get; set; }
    public string Content { get; set; }
    public bool Solved { get; set; }
    public UserViewModel? PostCreator { get; set; }
    public List<PostViewModel>? Tags { get; set; }
    public List<AnswerViewModel>? Answers { get; set; }
    public List<PostTagsViewModel>? PostTags { get; set; }
}