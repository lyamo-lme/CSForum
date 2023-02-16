using CSForum.Core.Models;

namespace CSForum.Shared.Models.ViewModels;

public class ChatViewModel
{
    public int ChatId { get; set; }
    public List<MessageViewModel>? Messages { get; set; }
}