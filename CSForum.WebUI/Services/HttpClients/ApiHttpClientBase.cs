using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using CSForum.Core;
using CSForum.Shared.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Microsoft.AspNetCore.Authentication;
using Polly.Retry;

namespace CSForum.Services.HttpClients;

public class ApiHttpClientBase : ApiClientBase
{
    private AsyncRetryPolicy _retryPolicy;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _contextAccessor;

    public ApiHttpClientBase(ITokenService tokenService, HttpClient client, IOptions<ApiSettingConfig> apiSettings,
        IHttpContextAccessor contextAccessor) : base(client,
        apiSettings.Value)
    {
        _contextAccessor = contextAccessor;
        _tokenService = tokenService;
        SetPolly();
    }

    public ApiHttpClientBase(ITokenService tokenService, IOptions<ApiSettingConfig> apiSettings,
        IHttpContextAccessor contextAccessor) : base(apiSettings.Value)
    {
        _contextAccessor = contextAccessor;
        _tokenService = tokenService;
        SetPolly();
    }

    private void SetPolly()
    {
        _retryPolicy = Policy.Handle<HttpRequestException>(exception =>
        {
            if (exception.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = _contextAccessor.HttpContext.GetTokenAsync("refresh_token").Result;
                var tokenResponse = _tokenService.RefreshAccessToken(refreshToken).Result;

                var authInfo = _contextAccessor.HttpContext.AuthenticateAsync("Cookie").Result;
                authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
                authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

                _contextAccessor.HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);

                SetBearerToken().Wait();
            }

            return true;
        }).RetryAsync(3);
    }


    public async Task<TDto> PostAsync<TDto>(TDto model, string? path = null, bool isAuth = false) where TDto : class
        => await PostAsync<TDto, TDto>(model, path, isAuth);


    public async Task<TOut> PostAsync<TDto, TOut>(TDto model, string? path = null, bool isAuth = false)
        where TOut : class
    {
        try
        {
            if (isAuth)
            {
                await SetBearerToken();
            }

            return await _retryPolicy.ExecuteAsync(async () =>
                await ExecuteRequestAsync<TDto, TOut>(HttpMethod.Post, path, model));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task SetBearerToken()
    {
        var accToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
        client.SetBearerToken(accToken);
    }


    public async Task<TOut> GetAsync<TOut>(string path, bool isAuth = false)
    {
        try
        {
            if (isAuth)
            {
                await SetBearerToken();
            }

            var result = await _retryPolicy.ExecuteAsync(async () =>
                await ExecuteRequestAsync<TOut>(HttpMethod.Get, path));
            
            return result;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task<TOut> GetDeserializeObject<TOut>(HttpResponseMessage response)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOut>(content);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task<TOut> ExecuteRequestAsync<TOut>(HttpMethod method, string path)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + path);
            HttpResponseMessage response = new HttpResponseMessage();
            switch (method.Method)
            {
                case "GET":
                {
                    response = await client.GetAsync(uri);
                    break;
                }
                case "DELETE":
                {
                    response = await client.DeleteAsync(uri);
                    break;
                }
                default:
                {
                    throw new Exception();
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.RequestMessage.ToString(),
                    new Exception(),
                    response.StatusCode);
            }

            return await GetDeserializeObject<TOut>(response);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task<TOut> ExecuteRequestAsync<TDto, TOut>(HttpMethod method, string path, TDto? model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + path);
            HttpResponseMessage response = new HttpResponseMessage();
            switch (method.Method)
            {
                case "POST":
                {
                    response = await client.PostAsJsonAsync(uri, model);
                    break;
                }
                case "PUT":
                {
                    response = await client.PutAsJsonAsync(uri, model);
                    break;
                }
                default:
                {
                    throw new Exception();
                }
            }

            return await GetDeserializeObject<TOut>(response);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}