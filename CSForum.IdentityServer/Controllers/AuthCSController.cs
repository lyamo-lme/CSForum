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

namespace CSForum.IdentityServer.Controllers;

public class AuthCSController : Controller
{
    private readonly IEmailService _emailSender;
    private readonly ILogger<AuthCSController> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IIdentityServerInteractionService _interactionService;

    public AuthCSController(SignInManager<User> signInManager, UserManager<User> userManager,
        IIdentityServerInteractionService interactionService, IEmailService emailSender, ILogger<AuthCSController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _interactionService = interactionService;
        _emailSender = emailSender;
        _logger = logger;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        try
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

            if (Url.IsLocalUrl(returnUrl))
            {
                return View("LoginPage", new LoginViewModel()
                {
                    ReturnUrl = returnUrl,
                    ExternalProviders = externalProviders
                });
            }

            return View("LoginPage", new LoginViewModel()
            {
                ReturnUrl = "",
                ExternalProviders = externalProviders
            });
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        try
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return View("RegisterPage", new RegisterViewModel()
                {
                    ReturnUrl = returnUrl
                });
            }

            return View("RegisterPage");
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        try
        {
            if (!Url.IsLocalUrl(model.ReturnUrl))
            {
                return View("RegisterPage");
            }

            if (!model.Password.Equals(model.ConfirmPassword) && !ModelState.IsValid)
            {
                return View("RegisterPage", model);
            }

            await _userManager.CreateAsync(
                _mapper.Map<User>(new IdentityUser<int>(model.Username)
                {
                    Email = model.Email
                }),
                model.Password);

            //await _emailSender.SendMessage<Email>(
            //            new Email()
            //            {
            //                senderName = "code?reply",
            //                receiverEmail = model.Email,
            //                htmlContent = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
            //                subject = "Confirming registration"
            //            });
            if (model.ReturnUrl != null)
            {
                return Redirect($"Login?ReturnUrl={model.ReturnUrl}");
            }

            return Redirect("Home/Index");
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            if (!Url.IsLocalUrl(model.ReturnUrl))
            {
                return View("LoginPage");
            }

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


            return View("LoginPage");
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Logout(string logoutId)
    {
        try
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    public async Task<IActionResult> ExternalProvider(string provider, string returnUrl)
    {
        try
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                return View("LoginPage");
            }

            var redirect = Url.Action(nameof(ExternalLoginCallback), "AuthCS", new
            {
                returnUrl
            });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);

            return Challenge(properties, provider);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    private async Task<bool> ExternalRegister(ExternalLoginInfo info)
    {
        try
        {
            var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            var userName = info.Principal.FindFirst(ClaimTypes.GivenName).Value;

            var user = _mapper.Map<User>(new IdentityUser<int>(userName)
            {
                Email = email
            });

            var resultRegister = await _userManager.CreateAsync(user);
            if (!resultRegister.Succeeded)
            {
                return false;
            }

            var loginResult = await _userManager.AddLoginAsync(user, info);
            if (!loginResult.Succeeded)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
    {
        try
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                return View("LoginPage");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction($"Login?returnUrl={returnUrl}");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                await _signInManager.SignInWithClaimsAsync(user,
                    false,
                    new[]
                    {
                        new Claim("Id", $"{user.Id}")
                    }
                );

                return Redirect(returnUrl);
            }

            await ExternalRegister(info);
            return Redirect(returnUrl);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }
}