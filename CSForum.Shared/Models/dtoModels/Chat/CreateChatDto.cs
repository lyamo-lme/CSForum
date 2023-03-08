using System.ComponentModel.DataAnnotations;

namespace CSForum.Shared.Models.dtoModels.Chat;

public class CreateChatDto
{
    public int UserId { get; set; }
    [Required]
    // [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
    public string Content { get; set; }
}