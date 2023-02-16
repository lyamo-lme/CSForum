using CSForum.WebUI.Models;

namespace CSForum.Shared.Models.ViewModels;

public class MessageViewModel
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public UserViewModel? User { get; set; }
    public ChatViewModel? Chat { get; set; }
}