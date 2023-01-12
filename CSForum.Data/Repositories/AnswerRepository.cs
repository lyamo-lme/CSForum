using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Repositories;

public class AnswerRepository:IAnswerRepository
{
    private ForumContext Context { get; set; }

    public AnswerRepository(ForumContext context)
    {
        Context = context;
    }

    public async Task<List<Answer>> GetAsync() => await Context.Answers.ToListAsync();

    public async Task<Answer> GetByIdAsync(int id) => await Context.Answers.FirstOrDefaultAsync(x=>x.Id==id);
   

    public async Task<Answer> CreateAsync(Answer model)
    {
        try
        {
            await Context.Answers.AddAsync(model);
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
            Context.Answers.Remove(new Answer()
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
}