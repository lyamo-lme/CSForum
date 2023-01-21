using System.Text;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Shared.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSForum.Services.HttpClients;

public class UserClient : IUserClient
{
    private HttpClient client;
    private ApiSettingConfig apiSettings;

    public UserClient(HttpClient client, IOptions<ApiSettingConfig> options)
    {
        this.client = client;
        apiSettings = options.Value;
        this.client.BaseAddress = new Uri(apiSettings.webApiUrl);
    }

    public async Task<User> CreateUser(User model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/users/create");
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, stringContent);
            return JsonConvert.DeserializeObject<User>(response.Content.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public Task<User> EditUser(User model)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUsers()
    {
        throw new NotImplementedException();
    }
}