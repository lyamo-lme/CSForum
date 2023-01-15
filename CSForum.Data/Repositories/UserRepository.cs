using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CSForum.Data.Repositories;

public class UserRepository : IUserRepository
{
    private ForumContext Context { get; set; }

    public UserRepository(ForumContext context)
    {
        Context = context;
    }

    public async Task<List<User>> GetAsync()
    {
        try
        {
            return await Context.Users.ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<User> GetFirstByFunc(Expression<Func<User,bool>> func)
    {
        try
        {
            return await Context.Users.FirstAsync(func);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<User> CreateAsync(User model)
    {
        try
        {
            await Context.Users.AddAsync(model);
            await Context.SaveChangesAsync();
            return model;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            Context.Users.Remove(new User()
            {
                UserId = id
            });
            await Context.SaveChangesAsync();
            return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}