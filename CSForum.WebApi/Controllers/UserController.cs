using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Services.EntityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUnitOfWorkRepository _uofRepository;

    public UserController(IUnitOfWorkRepository uofRepository)
    {
        _uofRepository = uofRepository;
    }

    [HttpGet]
    public async Task<List<User>> GetUsers()
    {
        try
        {
            return (await _uofRepository.GenericRepository<User>().GetAsync()).ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    [HttpGet]
    [Route("{id}/{additionalInfo}")]
    public async Task<ActionResult<User>> GetUser(int id, string additionalInfo)
    {
        try
        {
            if (!AllowedData.AllowedUserData.Contains(additionalInfo))
            {
                return BadRequest("Dont have access to this data");
            }

            var user = await _uofRepository.GenericRepository<User>().FindAsync(
                user => user.Id == id, additionalInfo.Equals("null") ? null : additionalInfo);
            
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    [HttpPost, Route("create")]
    public async Task<ActionResult<User>> CreateUser([FromBody] User model)
    {
        try
        {
            var user = await _uofRepository.GenericRepository<User>().CreateAsync(model);
            await _uofRepository.SaveAsync();
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    [HttpPost, Route("edit")]
    public async Task<ActionResult<User>> EditUser([FromBody] User model)
    {
        try
        {
            var user = await _uofRepository.GenericRepository<User>().UpdateAsync(model);
            await _uofRepository.SaveAsync();
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    [HttpDelete, Route("delete")]
    public async Task<ActionResult<bool>> DeleteUser(int userId)
    {
        try
        {
            var result = await _uofRepository.GenericRepository<User>().DeleteAsync(new User
            {
                Id = userId
            });
            await _uofRepository.SaveAsync();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}