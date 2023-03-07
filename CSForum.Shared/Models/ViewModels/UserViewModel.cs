using CSForum.WebUI.Models;

namespace CSForum.Shared.Models.ViewModels;

public class UserViewModel
{
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<PostViewModel>? Posts { get; set; } 
        public List<AnswerViewModel>? Answers { get; set; }
}