namespace CSForum.Shared.Models.dtoModels.Posts;

public class EditPostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool Solved { get; set; }
    public int UserId { get; set; }
}