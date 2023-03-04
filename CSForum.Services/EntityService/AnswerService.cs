using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;

namespace CSForum.Services.EntityService;

public class AnswerService : IAnswerService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly IUserService _userService;

    public AnswerService(IUnitOfWorkRepository uofRepository, IUserService userService)
    {
        _uofRepository = uofRepository;
        _userService = userService;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    public async Task<Answer> CreateAnswer(Answer model)
    {
        var answer = await _uofRepository.GenericRepository<Answer>().CreateAsync(model);

        var user = await _uofRepository.GenericRepository<User>().FindAsync(x => x.Id == model.UserId);
        user.RatingScores += 5;
        await _userService.UpdateUser(user);

        await _uofRepository.SaveAsync();
        return answer;
    }

    public async Task<Answer> UpdateAnswer(Answer model)
    {
        var answer = await _uofRepository.GenericRepository<Answer>().UpdateAsync(model);
        await _uofRepository.SaveAsync();
        return answer;
    }

    public async Task<Answer> UpdateStateAnswer(Answer model)
    {
        var user = await _uofRepository.GenericRepository<User>().FindAsync(x => x.Id == model.UserId);
        var ans = await _uofRepository.GenericRepository<Answer>().UpdateAsync(model);
        if (model.Accepted)
        {
            user.RatingScores += 10;
            var upt = await _uofRepository.GenericRepository<User>().UpdateAsync(user);
        }
        else
        {
            user.RatingScores -= 10;
            var upt = await _uofRepository.GenericRepository<User>().UpdateAsync(user);
        }

        await _uofRepository.SaveAsync();
        return ans;
    }
}