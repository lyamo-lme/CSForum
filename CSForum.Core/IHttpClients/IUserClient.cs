using CSForum.Core.Models;

namespace CSForum.Core.IHttpClients;

public interface IUserClient
{
    public Task<User> CreateUser(User model);
    public Task<User> EditUser(User model);
    public Task<bool> DeleteUser(string userId);
    public Task<List<User>> GetUsers();
}