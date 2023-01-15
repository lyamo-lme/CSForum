using CSForum.Core.IRepositories;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private ForumContext f { get; set; }
    private IUserRepository u { get; set; }

    public HomeController(ForumContext d, IUserRepository u)
    {
        f = d;
        this.u = u;
    }
    [HttpGet]
    public async Task<ActionResult<string>> GetConnection()
    {
        var model = await u.GetFirstByFunc(x => x.UserId==1);
        return Ok(f.Database.ToString());
    }
}