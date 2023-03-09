using System.Net;
using System.Net.Http.Json;
using CSForum.Core;
using CSForum.Core.Http;
using CSForum.Shared.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace CSForum.Services.Http;

/// <summary>
/// need to refactor:
///create only one public method than will provide access to use http requests 
/// </summary>
public class ApiHttpClientBase : ApiClientBase
{
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly ITokenService _tokenService;
    private readonly IHttpAuthorization _httpAuthorization;
    private readonly ILogger<ApiHttpClientBase> _logger;

    public ApiHttpClientBase(ITokenService tokenService, HttpClient hc, IOptions<ApiSettingConfig> apiSettings,
        IHttpAuthorization httpAuthorization, ILogger<ApiHttpClientBase> logger)
        : base(hc, apiSettings.Value)
    {
        _httpAuthorization = httpAuthorization;
        _logger = logger;
        _tokenService = tokenService;
        _retryPolicy = SetPolly();
    }

    private AsyncRetryPolicy SetPolly()
    {
        return Policy.Handle<HttpRequestException>(exception =>
        {
            if (exception.StatusCode == HttpStatusCode.Unauthorized)
            {
                string refreshToken = _httpAuthorization.GetToken("refresh_token").GetAwaiter().GetResult();
                var tokenResponse = _tokenService.RefreshAccessToken(refreshToken).GetAwaiter().GetResult();
                ;
                _httpAuthorization.UpdateTokens(new Dictionary<string, string>()
                {
                    { "access_token", tokenResponse.AccessToken },
                    { "refresh_token", tokenResponse.RefreshToken }
                });
                SetBearerTokenAsync().Wait();
            }

            return true;
        }).RetryAsync(3);
    }


    public async Task<TDto> PostAsync<TDto>(TDto model, string? path = null) where TDto : class
    {
        try
        {
            return await PostAsync<TDto, TDto>(model, path);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    public async Task<TOut> PutAsync<TDto, TOut>(TDto model, string path) where TOut : class
    {
        try
        {
            JsonContent content = JsonContent.Create(model);
            return await _retryPolicy.ExecuteAsync(async () =>
                await ExecuteRequestAsync<TOut>(HttpMethod.Put, path, content));
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    public async Task<TOut> PostAsync<TDto, TOut>(TDto model, string path) where TOut : class
    {
        try
        {
            JsonContent content = JsonContent.Create(model);
            return await _retryPolicy.ExecuteAsync(async () =>
                await ExecuteRequestAsync<TOut>(HttpMethod.Post, path, content));
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    public async Task SetBearerTokenAsync()
    {
        try
        {
            string accToken = await _httpAuthorization.GetToken("access_token");
            hc.SetBearerToken(accToken);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }


    public async Task<TOut> GetAsync<TOut>(string path)
    {
        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
                await ExecuteRequestAsync<TOut>(HttpMethod.Get, path));

            return result;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
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
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }


    private async Task<TOut> ExecuteRequestAsync<TOut>(HttpMethod method, string path)
    {
        try
        {
            var uri = new Uri(hc.BaseAddress + path);
            var result = await hc.SendAuthAsync(new HttpRequestMessage(method, uri));

            return await GetDeserializeObject<TOut>(result);
        }
        catch (HttpRequestException e)
        {
            throw new HttpRequestException("non auth", new Exception(e.Message), e.StatusCode);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    private async Task<TOut> ExecuteRequestAsync<TOut>(HttpMethod method, string path, HttpContent? model)
    {
        try
        {
            var uri = new Uri(hc.BaseAddress + path);
            var result = await hc.SendAuthAsync(
                new HttpRequestMessage(method, uri)
                {
                    Content = model
                }
            );

            return await GetDeserializeObject<TOut>(result);
        }
        catch (HttpRequestException e)
        {
            throw new HttpRequestException("non auth", new Exception(e.Message), e.StatusCode);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }
}