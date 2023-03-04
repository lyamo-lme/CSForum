using CSForum.Shared.Models.ViewModels;

namespace CSForum.WebUI.Models;

public class UserViewModel
{
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<PostViewModel>? Posts { get; set; } 
        public List<AnswerViewModel>? Answers { get; set; }
        public int? MenuOption { get; set; } = 0;
}