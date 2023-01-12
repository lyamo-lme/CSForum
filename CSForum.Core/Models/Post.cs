namespace CSForum.Core.Models;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime DateCreate { get; set; }  
    public string Content { get; set; }
}