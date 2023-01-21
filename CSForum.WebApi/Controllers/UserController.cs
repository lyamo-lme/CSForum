using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUserRepository userRepository;
    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    [HttpGet]
    public async Task<List<User>> GetUsers()
    {
        try
        {
            return await userRepository.GetAsync();
        }
        catch (Exception e )
        {
            throw new Exception(e.Message, e);
        }
    }
    [HttpPost, Route("create")]
    public async Task<User> CreateUser([FromBody]User model)
    {
        try
        {
            var user = await userRepository.CreateAsync(model);
            await userRepository.SaveChangesAsync();
            return user;
        }
        catch (Exception e )
        {
            throw new Exception(e.Message, e);
        }
    }
    [HttpPost, Route("edit")]
    public async Task<User> EditUser([FromBody]User model)
    {
        try
        {
            var user = await userRepository.UpdateAsync(model);
            await userRepository.SaveChangesAsync();
            return user;
        }
        catch (Exception e )
        {
            throw new Exception(e.Message, e);
        }
    }
    [HttpDelete, Route("delete")]
    public async Task<bool> DeleteUser(string userId)
    {
        try
        {
            var result = await userRepository.DeleteAsync(userId);
            await userRepository.SaveChangesAsync();
            return result;
        }
        catch (Exception e )
        {
            throw new Exception(e.Message, e);
        }
    }
}