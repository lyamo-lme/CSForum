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
}