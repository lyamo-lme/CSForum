using System.ComponentModel.DataAnnotations;

namespace CSForum.WebUI.Models;

public class CreateAnswerViewModel
{
    [Required]
    public int  PostId { get; set; }
    public int? ParentAnswerId { get; set; } = null;
    [Required]
    [MinLength(50)]
    public string Content { get; set; }
}