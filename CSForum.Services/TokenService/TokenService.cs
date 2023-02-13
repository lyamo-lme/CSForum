using CSForum.Core;
using CSForum.Shared;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using  CSForum.Core.Models;

namespace CSForum.Services.TokenService;

public class TokenService:ITokenService
{
    private readonly IdentityServerSettings _serverSettings;
    private readonly DiscoveryDocumentResponse _discoveyDocument;
    private readonly HttpClient _httpClient;

    public TokenService(IOptions<IdentityServerSettings> optionsIdentity)
    {
        _serverSettings = optionsIdentity.Value;
        _httpClient = new HttpClient();
        _discoveyDocument = _httpClient.GetDiscoveryDocumentAsync(_serverSettings.DiscoveryUrl).Result;
        
    }
    public async Task<TokenResponse> GetToken(string scope)
    {
        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = _discoveyDocument.TokenEndpoint,
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
        var discoveryDocument = await 
            _httpClient.GetDiscoveryDocumentAsync(_serverSettings.DiscoveryUrl);
        var requestDate = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken
        };
        var request = new HttpRequestMessage(HttpMethod.Post, _serverSettings.DiscoveryUrl);
        return new TokenResponse();
    }
}