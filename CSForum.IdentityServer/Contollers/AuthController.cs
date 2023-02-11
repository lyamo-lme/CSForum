using System.Security.Claims;
using System.Text.Encodings.Web;
using AutoMapper;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.IdentityServer.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.IdentityServer.Contollers;

public class AuthController : Controller
{
    private readonly IEmailService _emailSender;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IIdentityServerInteractionService _interactionService;

    public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, IIdentityServerInteractionService interactionService)
    {
        this._signInManager = signInManager;
        _userManager = userManager;
        _interactionService = interactionService;
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
            if (!model.Password.Equals(model.ConfirmPassword) && !ModelState.IsValid)
            {
                return View("RegisterPage", model);
            }

            await _userManager.CreateAsync(
                _mapper.Map<User>(new IdentityUser<int>(model.Username)),
                model.Password);

            //await _emailSender.SendMessage<Email>(
            //            new Email()
            //            {
            //                senderName = "code?reply",
            //                receiverEmail = model.Email,
            //                htmlContent = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
            //                subject = "Confirming registration"
            //            });

            return Redirect($"Login?ReturnUrl={model.ReturnUrl}");
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("LoginPage", new LoginViewModel()
            {
                ReturnUrl = model.ReturnUrl
            });
        }

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

    [HttpGet, Authorize]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
        return Redirect(logoutRequest.PostLogoutRedirectUri);
    }

}