using Microsoft.AspNetCore.Identity;

namespace CSForum.Core.Models;

public class User:IdentityUser<int>
{
    public int? RatingScores { get; set; }
    public List<Post>? Posts { get; set; } 
    public List<Answer>? Answers { get; set; }
    public List<Chat>? Chats { get; set; }
    public List<Message>? Messages { get; set; }
}