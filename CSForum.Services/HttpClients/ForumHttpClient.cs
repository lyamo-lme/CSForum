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

public class ForumHttpClient : TypedApiClient, IForumClient
{
    public ForumHttpClient(HttpClient client, IOptions<ApiSettingConfig> apiSettings) : base(client, apiSettings.Value)
    { }
    public ForumHttpClient(IOptions<ApiSettingConfig> apiSettings) : base(apiSettings.Value)
    { }

    public async Task<TDto> PostAsync<TDto>(TDto model, string? path = null)
        => await PostAsync<TDto, TDto>(model, path);

    public async Task<TOut> PostAsync<TDto,TOut>(TDto model, string? path=null)
    {
        try
        {
            var uri = new Uri(client.BaseAddress+path);
            var response = await client.PostAsJsonAsync(uri,model);
            return JsonConvert.DeserializeObject<TOut>(response.Content.ToString());
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
            return JsonConvert.DeserializeObject<TOut>(response.Content.ToString());
        }
        catch(Exception e)
        {
            throw;
        }
    }
}