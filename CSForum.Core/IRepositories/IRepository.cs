using System.Linq.Expressions;
using CSForum.Core.Models;

namespace CSForum.Core.IRepositories;

public interface IRepository<T>
{
    public Task<List<T>> GetAsync();
    public Task<T> GetFirstByFunc(Expression<Func<T, bool>> func);
    public Task<T> CreateAsync(T model);
    public Task<bool> DeleteAsync(int id);
    public Task<T> UpdateAsync(T model);
}