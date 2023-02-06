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

public class ForumHttpClientBase : ApiClientBase, IForumClient
{
    public ForumHttpClientBase(HttpClient client, IOptions<ApiSettingConfig> apiSettings) : base(client, apiSettings.Value)
    { }
    public ForumHttpClientBase(IOptions<ApiSettingConfig> apiSettings) : base(apiSettings.Value)
    { }

    public async Task<TDto> PostAsync<TDto>(TDto model, string? path = null)
        => await PostAsync<TDto, TDto>(model, path);

    public async Task<TOut> PostAsync<TDto,TOut>(TDto model, string? path=null)
    {
        try
        {
            var uri = new Uri(client.BaseAddress+path);
            var response = await client.PostAsJsonAsync(uri,model);
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
            var uri = new Uri(client.BaseAddress+path);
            var response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOut>(content);
        }
        catch(Exception e)
        {
            throw;
        }
    }
}