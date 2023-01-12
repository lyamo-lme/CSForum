namespace CSForum.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool? Gender { get; set; }
    public DateTime DateCreate { get; set; }  
}