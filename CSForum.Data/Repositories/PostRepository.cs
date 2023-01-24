using System.Linq.Expressions;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class PostRepository : IRepository<Post>
{
    private ForumDbContext Context { get; set; }

    public PostRepository(ForumDbContext forumContext)
    {
        Context = forumContext;
    }
    public async Task<IEnumerable<Post>> GetByFuncExpAsync(Func<Post, bool> func)
    {
        try
        {
            var queryable = Context.Posts.AsQueryable();
            var  posts =  queryable.Where(func);
            return posts;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<IEnumerable<Post>> GetAsync()
    {
        try
        {
            return await Context.Posts.ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public  async Task<Post> FindAsync(Expression<Func<Post, bool>> func)
    {
        try
        {
            return await Context.Posts.FirstAsync(func);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    public async Task<Post> CreateAsync(Post model)
    {
        try
        {
             await Context.Posts.AddAsync(model);
             return model;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message) ;
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
            throw new Exception(e.Message) ;
        }
    }

    public async Task<Post> UpdateAsync(Post model)
    {
        try
        {
            var updModel = Context.Posts.Update(model);
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