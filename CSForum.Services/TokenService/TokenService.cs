using System.Text;
using CSForum.Core;
using CSForum.Shared;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using  CSForum.Core.Models;
using Newtonsoft.Json;

namespace CSForum.Services.TokenService;

public class TokenService:ITokenService
{
    private readonly IdentityServerSettings _serverSettings;
    private readonly DiscoveryDocumentResponse _discoveryDocument;
    private readonly HttpClient _httpClient;

    public TokenService(IOptions<IdentityServerSettings> optionsIdentity)
    {
        _serverSettings = optionsIdentity.Value;
        _httpClient = new HttpClient();
        _discoveryDocument = _httpClient.GetDiscoveryDocumentAsync(_serverSettings.DiscoveryUrl).Result;
        
    }
    public async Task<TokenResponse> GetToken(string scope)
    {
        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = _discoveryDocument.TokenEndpoint,
                ClientId = _serverSettings.ClientName,
                ClientSecret = _serverSettings.ClientPassword,
                Scope = scope
            }
            );
        if(tokenResponse.IsError){
            throw new Exception();
        }

        return tokenResponse;
    }

    public async Task<TokenResponse> RefreshAccessToken(string refreshToken)
    {
        
       var tokenResponse = await   _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = _discoveryDocument.TokenEndpoint,
            RefreshToken = refreshToken,
            ClientId = _serverSettings.ClientName,
            ClientSecret = _serverSettings.ClientPassword,
        });

       return tokenResponse;
    }
}