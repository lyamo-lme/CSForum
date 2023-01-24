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

    public async Task<Tag> CreateTag(Tag model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/api/tags/create");
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

    public Task<Tag> EditTag(Tag model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTag(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Tag>> GetTag()
    {
        throw new NotImplementedException();
    }
}