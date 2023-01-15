namespace CSForum.Core.Models;

public class User
{
    public int UserId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool? Gender { get; set; }
    public DateTime DateCreate { get; set; }
    public List<Post>? Posts { get; set; } 
    public List<Answer>? Answers { get; set; }
}