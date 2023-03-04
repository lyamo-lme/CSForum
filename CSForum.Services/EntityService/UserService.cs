using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;

namespace CSForum.Services.EntityService;

public class UserService:IUserService
{
    private readonly IUnitOfWorkRepository _uofRepository;
    public UserService(IUnitOfWorkRepository uofRepository)
    {
        _uofRepository = uofRepository;
    }
    public async Task<User> UpdateUser(User model)
    {
        try
        {
            return await _uofRepository.GenericRepository<User>().UpdateAsync(model);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}