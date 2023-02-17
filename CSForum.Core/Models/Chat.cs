namespace CSForum.Core.Models;

public class Chat
{
    public int ChatId { get; set; }
    public List<Message>? Messages { get; set; }
    public List<UsersChats>? UsersChats { get; set; }
    public List<User> Users { get; set; }
}