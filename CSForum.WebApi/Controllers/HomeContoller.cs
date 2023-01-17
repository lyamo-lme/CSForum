using CSForum.Core.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeContoller : ControllerBase
    {
        public IUserRepository u;
        public HomeContoller(IUserRepository u)
        {
            this.u = u;
        }
        [HttpGet]
        public async Task<string> Get()
        {
            var d = await u.GetAsync();
            return "Ok";
        }
    }
}