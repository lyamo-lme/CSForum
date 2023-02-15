using CSForum.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace CSForum.WebUI.Services.HttpClients;

public class HttpAuthorization : IHttpAuthorization
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpAuthorization(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }


    public async Task<string> GetToken(string nameRefreshToken)
    {
        try
        {
            return await _contextAccessor.HttpContext.GetTokenAsync(nameRefreshToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task UpdateTokens(Dictionary<string, string> tokens)
    {
        var authInfo = await _contextAccessor.HttpContext.AuthenticateAsync("Cookie");
        foreach (var token in tokens)
        {
            authInfo.Properties.UpdateTokenValue(token.Key, token.Value);
            authInfo.Properties.UpdateTokenValue(token.Key, token.Value);
        }

        await _contextAccessor.HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);
    }
}