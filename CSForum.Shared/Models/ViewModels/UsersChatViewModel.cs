using CSForum.WebUI.Models;

namespace CSForum.Shared.Models.ViewModels;

public class UsersChatViewModel
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public UserViewModel? User { get; set; }
    public ChatViewModel? Chat { get; set; }
}