using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using CSForum.Core;
using CSForum.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Microsoft.AspNetCore.Authentication;
using Polly.Retry;

namespace CSForum.Services.HttpClients;

public class ApiHttpClientBase : ApiClientBase
{
    private  AsyncRetryPolicy _retryPolicy;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _contextAccessor;

    public ApiHttpClientBase(ITokenService tokenService,HttpClient client, IOptions<ApiSettingConfig> apiSettings, IHttpContextAccessor contextAccessor) : base(client,
        apiSettings.Value)
    {
        _contextAccessor = contextAccessor;
        _tokenService = tokenService;
        SetPolly();
    }

    public ApiHttpClientBase(ITokenService tokenService,IOptions<ApiSettingConfig> apiSettings,  IHttpContextAccessor contextAccessor) : base(apiSettings.Value)
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
                
            }

            Console.Write("here");

            return false;
        }).RetryAsync(3);
    }

    
    public async Task<TDto> PostAsync<TDto>(TDto model, string? path = null) where TDto : class
        => await PostAsync<TDto, TDto>(model, path);


    public async Task<TOut> PostAsync<TDto, TOut>(TDto model, string? path = null) where TOut : class
    {
        try
        {
            return await ExecuteAsync(async ()
                => await ExecuteRequestAsync<TDto, TOut>(HttpMethod.Post, path, model));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<TOut> GetAsync<TOut>(string path)
    {
        try
        {
            return await ExecuteAsync(async ()
                => await ExecuteRequestAsync<TOut>(HttpMethod.Get, path));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task<TOut> GetDeserializeObject<TOut>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TOut>(content);
    }

    private async Task<TOut> ExecuteRequestAsync<TOut>(HttpMethod method, string path)
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
        return await GetDeserializeObject<TOut>(response);
    }
    
    private async Task<TOut> ExecuteRequestAsync<TDto, TOut>(HttpMethod method, string path, TDto? model)
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

    private async Task<TOut> ExecuteAsync<TOut>(Func<Task<TOut>> function)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(function);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}