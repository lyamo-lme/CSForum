using System.Linq.Expressions;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class TagRepository : IRepository<Tag>
{
    private ForumContext Context { get; set; }

    public TagRepository(ForumContext context)
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

    public Task<Tag> GetFirstByFunc(Expression<Func<Tag, bool>> func)
    {
        throw new NotImplementedException();
    }

    public Task<Tag> CreateAsync(Tag model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Tag> UpdateAsync(Tag model)
    {
        throw new NotImplementedException();
    }
}