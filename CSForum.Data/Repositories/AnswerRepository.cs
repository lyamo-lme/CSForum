using System.Linq.Expressions;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private IAnswerRepository _answerRepositoryImplementation;
    private ForumDbContext Context { get; set; }

    public AnswerRepository(ForumDbContext context)
    {
        Context = context;
    }

    public async Task<List<Answer>> GetAsync()
    {
        try
        {
          return  await Context.Answers.ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Answer> FindAsync(Expression<Func<Answer, bool>> func)
    {
        try
        {
            return await Context.Answers.FirstAsync(func);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    public async Task<Answer> CreateAsync(Answer model)
    {
        try
        {
            await Context.Answers.AddAsync(model);
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
            Context.Answers.Remove(new Answer()
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

    public async Task<Answer> UpdateAsync(Answer model)
    {
        try
        {
            var updModel = Context.Answers.Update(model);
            return updModel.Entity;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task SaveChanges()
    {
            await Context.SaveChangesAsync();
    }
}