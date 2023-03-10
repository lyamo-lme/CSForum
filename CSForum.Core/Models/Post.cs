namespace CSForum.Core.Models;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public DateTime DateCreate { get; set; } = DateTime.Now;
    public string Content { get; set; }
    public bool Solved { get; set; }
    public User? PostCreator { get; set; }
    public List<Tag>? Tags { get; set; }
    public List<Answer>? Answers { get; set; }
    public List<PostTag>? PostTags { get; set; }
}