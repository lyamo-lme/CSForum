namespace CSForum.WebUI.Models;

public class CreateAnswerViewModel
{
    public int  PostId { get; set; }
    public int? ParentAnswerId { get; set; } = null;
    public string Content { get; set; }
}