using System.Net.Http.Json;
using System.Text;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Shared.Models;
using CSForum.Shared.Models.dtoModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSForum.Services.HttpClients;

public class ApiHttpClientBase : ApiClientBase
{
    public ApiHttpClientBase(HttpClient client, IOptions<ApiSettingConfig> apiSettings) : base(client,
        apiSettings.Value)
    {
    }

    public ApiHttpClientBase(IOptions<ApiSettingConfig> apiSettings) : base(apiSettings.Value)
    {
    }

    public async Task<TDto> PostAsync<TDto>(TDto model, string? path = null) where TDto : class
        => await PostAsync<TDto, TDto>(model, path);


    public async Task<TOut> PostAsync<TDto, TOut>(TDto model, string? path = null) where TOut : class
    {
        try
        {
            var uri = new Uri(client.BaseAddress + path);
            var response = await client.PostAsJsonAsync(uri, model);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TOut>(content);
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
            var uri = new Uri(client.BaseAddress + path);
            var response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOut>(content);
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