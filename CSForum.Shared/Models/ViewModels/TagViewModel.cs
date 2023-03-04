using CSForum.Shared.Models.ViewModels;

namespace CSForum.WebUI.Models;

public class TagViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<PostViewModel>? Posts { get; set; }
}