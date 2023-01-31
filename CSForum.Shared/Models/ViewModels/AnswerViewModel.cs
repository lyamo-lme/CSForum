namespace CSForum.WebUI.Models;

public class AnswerViewModel
{
    public int Id { get; set; }
    public int  PostId { get; set; }
    public int? ParentAnswerId { get; set; }
    public int UserId { get; set; }
    public bool Accepted { get; set; }
    public DateTime DateCreate { get; set; }  
    public string ContentBody { get; set; }
    public UserViewModel? AnswerCreator { get; set; }
    public PostViewModel? Post { get; set; }
}