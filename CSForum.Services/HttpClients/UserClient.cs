using System.Text;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Shared.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSForum.Services.HttpClients;

public class UserClient :TypedApiClient, IUserClient
{


    public UserClient(HttpClient client, IOptions<ApiSettingConfig> apiSettings):base(client,apiSettings)
    {
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

    public async Task<User> EditUser(User model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/users/edit");
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

    public async Task<bool> DeleteUser(string userId)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + $"api/users/delete?userId={userId}");
            var response = await client.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<List<User>> GetUsers()
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/users");
            var response = await client.GetAsync(uri);
            return JsonConvert.DeserializeObject<List<User>>(response.Content.ToString());
        }
        catch(Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}