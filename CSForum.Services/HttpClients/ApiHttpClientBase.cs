using System.Net;
using System.Net.Http.Json;
using CSForum.Shared.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace CSForum.Services.HttpClients;

public class ApiHttpClientBase : ApiClientBase
{
    private AsyncRetryPolicy _retryPolicy;
    private HttpMethod _httpMethod;

    public ApiHttpClientBase(HttpClient client, IOptions<ApiSettingConfig> apiSettings) : base(client,
        apiSettings.Value)
    {
        SetPolly();
    }

    public ApiHttpClientBase(IOptions<ApiSettingConfig> apiSettings) : base(apiSettings.Value)
    {
        SetPolly();
    }

    private void SetPolly()
    {
        _retryPolicy = Policy.Handle<HttpRequestException>(exception =>
        {
            if (exception.StatusCode == HttpStatusCode.Unauthorized)
            {
                //need to refresh token
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

    /*idea to organize*/
    // public async Task<TDto> PostAsync<TDto>(TDto model, string? path = null) where TDto : class
    //     => await PostObjAsync<TDto, TDto>(model, path);
    //
    // public async Task<TDto> PostObjAsync<TDto>(TDto model, string? path = null) where TDto : struct
    //     => await PostStrAsync<TDto, TDto>(model, path);
    //
    // private async Task<TOut> PostStrAsync<TDto, TOut>(TDto model, string? path = null) where TOut : struct
    //     => (TOut)Convert.ChangeType(await PostRequestContent(model, path), typeof(TOut));
    //
    // private async Task<TOut> PostObjAsync<TDto, TOut>(TDto model, string? path = null) where TOut : class
    //     => JsonConvert.DeserializeObject<TOut>(await PostRequestContent(model, path));
    //
    //
    // private async Task<string> PostRequestContent<TDto>(TDto model, string? path = null)
    // {
    //     var uri = new Uri(client.BaseAddress + path);
    //     var response = await client.PostAsJsonAsync(uri, model);
    //     var content = await response.Content.ReadAsStringAsync();
    //     return content;
    // }
}