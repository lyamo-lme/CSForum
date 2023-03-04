using CSForum.Core.Models;

namespace CSForum.Core.Service;

public interface IUserService
{
    public Task<User> UpdateUser(User model);
}