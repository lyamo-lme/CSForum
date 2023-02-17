using CSForum.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace CSForum.WebUI.Services.HttpClients;

public class HttpAuthorization : IHttpAuthorization
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<HttpAuthorization> _logger;

    public HttpAuthorization(IHttpContextAccessor contextAccessor, ILogger<HttpAuthorization> logger)
    {
        _contextAccessor = contextAccessor;
        _logger = logger;
    }


    public async Task<string> GetToken(string nameRefreshToken)
    {
        try
        {
            return await _contextAccessor.HttpContext.GetTokenAsync(nameRefreshToken);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    public async Task UpdateTokens(Dictionary<string, string> tokens)
    {
        try
        {
            var authInfo = await _contextAccessor.HttpContext.AuthenticateAsync("Cookie");
            foreach (var token in tokens)
            {
                authInfo.Properties.UpdateTokenValue(token.Key, token.Value);
            }

            await _contextAccessor.HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }
}