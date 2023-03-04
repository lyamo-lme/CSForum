namespace CSForum.Shared.Models.dtoModels.Answer;

public class EditAnswerDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public bool Accepted { get; set; }
    public string ContentBody { get; set; } = "";
}