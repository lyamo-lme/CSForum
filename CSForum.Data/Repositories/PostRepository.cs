using System.Linq.Expressions;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class PostRepository : IPostRepository
{
    private ForumContext Context { get; set; }

    public PostRepository(ForumContext forumContext)
    {
        Context = forumContext;
    }

    public async Task<List<Post?>> GetAsync()
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

    public  async Task<Post> GetFirstByFunc(Expression<Func<Post, bool>> func)
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
             await Context.SaveChangesAsync();
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
            await Context.SaveChangesAsync();
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
            await Context.SaveChangesAsync();
            return updModel.Entity;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}