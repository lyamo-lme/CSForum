namespace CSForum.Shared.Models.dtoModels;

public class CreateAnswer
{
    public int? ParentAnswerId { get; set; } = null;
    public string Content { get; set; }
}