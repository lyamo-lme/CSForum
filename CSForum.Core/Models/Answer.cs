namespace CSForum.Core.Models;

public class Answer
{
    public int Id { get; set; }
    public int  PostId { get; set; }
    public int? ParentAnswerId { get; set; }
    public int UserId { get; set; }
    public bool Accepted { get; set; }
    public DateTime DateCreate { get; set; }  
    public string Content { get; set; }
    public User? AnswerCreator { get; set; }
    public Post? Post { get; set; }
}