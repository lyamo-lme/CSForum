namespace CSForum.Shared.Models.dtoModels.Answer;

public class CreateAnswerDto
{
    public int  PostId { get; set; }
    public int? ParentAnswerId { get; set; }
    public int UserId { get; set; }
    public bool Accepted { get; set; }
    public DateTime DateCreate { get; set; }  = DateTime.Now;
    public string ContentBody { get; set; } = "";
}