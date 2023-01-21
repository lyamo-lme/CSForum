using CSForum.Core.Models;

namespace CSForum.Core.IHttpClients;

public interface IUserClient
{
    public Task<User> CreateUser(User model);
    public Task<User> EditUser(User model);
    public Task<User> DeleteUser(string userId);
    public Task<User> GetUsers();
}