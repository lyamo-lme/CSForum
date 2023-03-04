using CSForum.Core.Models;

namespace CSForum.Core.Service;

public interface IAnswerService
{
    public Task<Answer> CreateAnswer(Answer model);
    public Task<Answer> UpdateAnswer(Answer model);
    public Task<Answer> UpdateStateAnswer(Answer model);
}