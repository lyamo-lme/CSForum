namespace CSForum.Core.Models;

public class Answer
{
    public int Id { get; set; }
    public int  PostId { get; set; }
    public int? ParentAnswerId { get; set; }
    public int UserId { get; set; }
    public DateTime DateCreate { get; set; }  
    public string Content { get; set; }
}