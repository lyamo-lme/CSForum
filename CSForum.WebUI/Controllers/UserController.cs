using AutoMapper;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Http;
using CSForum.Services.HttpClients;
using CSForum.Services.MapperConfigurations;
using CSForum.WebUI.Models;
using CSForum.WebUI.Services.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class UserController : Controller
{
    private readonly ApiHttpClientBase _forumClient;
    private readonly IMapper _mapper;

    public UserController(ApiHttpClientBase forumClient)
    {
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
        _forumClient = forumClient;
    }

    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = _mapper.Map<UserViewModel>(
                await _forumClient.GetAsync<User>($"api/users/{id}"));
            return View("UserView", user);
        }
        catch(Exception e)
        {
            throw;
        }
    }
}