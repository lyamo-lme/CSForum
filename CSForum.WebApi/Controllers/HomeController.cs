using CSForum.Core.IRepositories;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class HomeController : ControllerBase
{
    private IUserRepository u { get; set; }
    public HomeController(IUserRepository u)
    {
        this.u = u;
    }
    [HttpGet]
    public async Task<ActionResult<string>> GetConnection()
    {
        return Ok("Ok");
    }
}