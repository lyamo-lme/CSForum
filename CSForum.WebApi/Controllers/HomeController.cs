using CSForum.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private ForumContext f { get; set; }

    public HomeController(ForumContext d)
    {
        f = d;
    }
    [HttpGet]
    public ActionResult<string> GetConnection()
    {
        return Ok(f.Database.ToString());
    }
}