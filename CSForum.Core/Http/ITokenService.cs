using CSForum.Core.Models;
using IdentityModel.Client;

namespace CSForum.Core;

public interface ITokenService
{
    Task<TokenResponse> GetToken(string scope);
    Task<TokenResponse> RefreshAccessToken(string refreshToken);
}