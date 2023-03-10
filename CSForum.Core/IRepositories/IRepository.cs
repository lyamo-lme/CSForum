using CSForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Core.IRepositories;

public interface IRepository<TEntity> where TEntity:class
{
    public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,int? take=null, int? skip=null,
        string includeProperties = "");
    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> func, string? include=null);
    public ValueTask<TEntity> CreateAsync(TEntity model);
    public Task<bool> DeleteAsync(TEntity entity);
    public ValueTask<TEntity> UpdateAsync(TEntity model);
}