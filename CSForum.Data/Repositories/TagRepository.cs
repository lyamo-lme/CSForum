using System.Linq.Expressions;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class TagRepository : ITagRepository
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

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            Context.Posts.Remove(new Post()
            {
                Id = id
            });
            return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Tag> UpdateAsync(Tag model)
    {
        try
        {
            var updModel = Context.Tags.Update(model);
            return updModel.Entity;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}