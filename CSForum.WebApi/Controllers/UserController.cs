using CSForum.Core.IRepositories;
using CSForum.Core.Models;
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
    [HttpGet,Route("secret"),Authorize]
    public  IActionResult Secret()
    {
        return Ok("success");
    }
    [HttpGet]
    public async Task<List<User>> GetUsers()
    {
        try
        {
            return await _uofRepository.GenericRepository<User>().GetAsync();
        }
        catch (Exception e )
        {
            throw new Exception(e.Message, e);
        }
    }
    [HttpGet, Route("{id}")]
    public async Task<User?> GetUser(int id)
    {
        try
        {
            return await _uofRepository.GenericRepository<User>().FindAsync(x => x.Id == id);
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
            var user = await _uofRepository.GenericRepository<User>().CreateAsync(model);
            await _uofRepository.SaveAsync();
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
            var user = await  _uofRepository.GenericRepository<User>().UpdateAsync(model);
            await _uofRepository.SaveAsync();
            return user;
        }
        catch (Exception e )
        {
            throw new Exception(e.Message, e);
        }
    }
    [HttpDelete, Route("delete")]
    public async Task<bool> DeleteUser(int userId)
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
        catch (Exception e )
        {
            throw new Exception(e.Message, e);
        }
    }
}