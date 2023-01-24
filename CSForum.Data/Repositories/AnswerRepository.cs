using System.Linq.Expressions;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class AnswerRepository : IRepository<Answer>  
{
    private IRepository<Answer> _repositoryImplementation;
    private ForumDbContext Context { get; set; }

    public AnswerRepository(ForumDbContext context)
    {
        Context = context;
    }

    public async Task<IEnumerable<Answer>> GetAsync()
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

    public async Task<IEnumerable<Answer>> GetByFuncExpAsync(Func<Answer, bool> func)
    {
        try
        {
            var queryableAnswers = Context.Answers.AsQueryable();
            var  answers =  queryableAnswers.Where(func);
            return answers;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
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

    public async Task SaveChangesAsync()
    {
            await Context.SaveChangesAsync();
    }

}