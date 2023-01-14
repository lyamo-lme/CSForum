namespace CSForum.Core.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    private List<Post> Posts { get; set; }
}