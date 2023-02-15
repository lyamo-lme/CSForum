namespace CSForum.Core.Models;

public class Chat
{
    public int ChatId { get; set; }
    public int FirstUserId { get;set;}
    public int SecondUserId { get; set; }
    public List<Message>? Messages { get; set; }
    public User? FirstUser { get; set; }
    public User? SecondUser { get; set; }
    
}