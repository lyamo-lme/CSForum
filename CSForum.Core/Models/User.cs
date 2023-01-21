using Microsoft.AspNetCore.Identity;

namespace CSForum.Core.Models;

public class User:IdentityUser
{
    public List<Post>? Posts { get; set; } 
    public List<Answer>? Answers { get; set; }
}