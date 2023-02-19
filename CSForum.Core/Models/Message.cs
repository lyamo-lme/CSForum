namespace CSForum.Core.Models;


public class Message
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }= DateTime.Now;
    public User? User { get; set; }
    public Chat? Chat { get; set; }

    public Message()
    { }
    public Message(int userId, string content)
    {
        UserId = userId;
        Content = content;
    }
}