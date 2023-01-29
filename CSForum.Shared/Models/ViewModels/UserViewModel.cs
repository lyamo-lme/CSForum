namespace CSForum.WebUI.Models;

public class UserViewModel
{
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PhoneNumber { get; set; }
        public List<PostViewModel>? Posts { get; set; } 
        public List<AnswerViewModel>? Answers { get; set; }
}