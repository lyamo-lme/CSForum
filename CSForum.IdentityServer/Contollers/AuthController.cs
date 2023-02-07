using AutoMapper;
using CSForum.Core.Models;
using CSForum.IdentityServer.Models;
using CSForum.Services.MapperConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.IdentityServer.Contollers;

public class AuthController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        this._signInManager = signInManager;
        _userManager = userManager;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        return View("LoginPage", new LoginViewModel()
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        return View("RegisterPage", new RegisterViewModel()
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        try
        {
            if (model.Password.Equals(model.ConfirmPassword) || !ModelState.IsValid)
            {
                return View("RegisterPage", model);
            }

            await _userManager.CreateAsync(
                _mapper.Map<User>(new IdentityUser(model.Username)),
                model.Password);

            return Login(model.ReturnUrl);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
        if (result.Succeeded)
        {
            return Redirect(model.ReturnUrl);
        }
        else if (result.IsLockedOut)
        {
        }

        return View("LoginPage");
    }
}