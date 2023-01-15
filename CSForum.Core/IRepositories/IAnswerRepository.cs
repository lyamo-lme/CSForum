using System.Linq.Expressions;
using CSForum.Core.Models;

namespace CSForum.Core.IRepositories;

public interface IAnswerRepository
{
    public Task<List<Answer>> GetAsync();
    public Task<Answer>  GetFirstByFunc(Expression<Func<Answer, bool>> func);
    public Task<Answer> CreateAsync(Answer model);
    public Task<bool> DeleteAsync(int id);
    public Task<Answer> UpdateAsync(Answer model);
}