namespace CSForum.Shared.dtoModels;

public class CreatePost
{
    public string UserId { get; set; }
    public int Title { get; set; }
    public DateTime DateCreate { get; set; }  
    public string Content { get; set; }
}