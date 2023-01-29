using System.Linq.Expressions;
using System.Net.Mime;
using CSForum.Core.IRepositories;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ForumDbContext _context;
    private readonly DbSet<TEntity> _entity;

    public GenericRepository(ForumDbContext context)
    {
        this._context = context;
        _entity = context.Set<TEntity>();
    }

    public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        try
        {
            IQueryable<TEntity> query = _entity;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func)
    {
        try
        {
            var result = await _entity.FirstAsync(func);
            return result;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public Task<TEntity> CreateAsync(TEntity model)
    {
        try
        {
            var md = _entity.Add(model);
            return Task.FromResult(model);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public Task<bool> DeleteAsync(TEntity entityToDelete)
    {
        try
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _entity.Attach(entityToDelete);
            }

            _entity.Remove(entityToDelete);
            return Task.FromResult(true);
        }
        catch (Exception e)
        {
            throw;
        }
    }


    public  Task<TEntity> UpdateAsync(TEntity model)
    {
        try
        {
            return Task.Run(() =>
            {
                var entity = _entity.Update(model);
                _context.Entry(model).State = EntityState.Modified;
                return entity.Entity;
            });
        }
        catch (Exception e)
        {
            throw;
        }
    }
}