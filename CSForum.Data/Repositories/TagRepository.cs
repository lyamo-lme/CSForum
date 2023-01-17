using System.Linq.Expressions;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class TagRepository : IRepository<Tag>
{
    private ForumDbContext Context { get; set; }

    public TagRepository(ForumDbContext context)
    {
        Context = context;
    }

    public async Task<List<Tag>> GetAsync()
    {
        try
        {
            return await Context.Tags.ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Tag> FindAsync(Expression<Func<Tag, bool>> func)
    {
        try {
            return await Context.Tags.FirstAsync(func);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<Tag> CreateAsync(Tag model)
    {
        try
        {
            await Context.Tags.AddAsync(model);
            return model;
        }
        catch (Exception e) { 
            throw new Exception(e.Message, e);
        }
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Tag> UpdateAsync(Tag model)
    {
        throw new NotImplementedException();
    }
    public async Task SaveChanges()
    {
        await Context.SaveChangesAsync();
    }
}