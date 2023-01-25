using System.Reflection;
using System.Text;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Shared.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSForum.Services.HttpClients;

public class TagClient : TypedApiClient, ITagClient
{
    public TagClient(HttpClient client, IOptions<ApiSettingConfig> apiSettings) : base(client, apiSettings.Value)
    {
    }

    public async Task<Tag> CreateAsync<T>(T model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/tags/create");
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, stringContent);
            return JsonConvert.DeserializeObject<Tag>(response.Content.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<Tag> EditAsync<TDto>(TDto model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/tags/edit");
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, stringContent);
            return JsonConvert.DeserializeObject<Tag>(response.Content.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<bool> DeleteAsync(int tagId)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + $"api/tags/delete?tagId={tagId}");
            var response = await client.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<List<Tag>> GetAsync()
    {
        try
        {
            var uri = new Uri(client.BaseAddress +"api/tags");
            var response = await client.GetAsync(uri);
            return JsonConvert.DeserializeObject<List<Tag>>(response.Content.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }


    public async Task<Tag> FindAsync(int tagId)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + $"api/tags/tagId/{tagId}");
            var response = await client.GetAsync(uri);
            return JsonConvert.DeserializeObject<Tag>(response.Content.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}