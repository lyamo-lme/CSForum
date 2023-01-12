using CSForum.Core.Models;

namespace CSForum.Core.IRepositories;

public interface IAnswerRepository
{
    public Task<List<Answer>> GetAsync();
    public Task<Answer> GetByIdAsync(int id);
    public Task<Answer> CreateAsync(Answer model);
    public Task<bool> DeleteAsync(int id);
}