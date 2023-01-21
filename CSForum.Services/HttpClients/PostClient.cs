using System.Text;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Shared.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSForum.Services.HttpClients;

public class PostClient:TypedApiClient,IPostClient
{
    
    public PostClient(HttpClient client, IOptions<ApiSettingConfig> apiSettings):base(client, apiSettings)
    {
    }
    public async Task<Post> CreatePost(Post model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/posts/create");
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, stringContent);
            return JsonConvert.DeserializeObject<Post>(response.Content.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<Post> EditPost(Post model)
    {
        try
        {
            var uri = new Uri(client.BaseAddress +"api/posts/edit");
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, stringContent);
            return JsonConvert.DeserializeObject<Post>(response.Content.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<bool> DeletePost(string postId)
    {
        try
        {
            var uri = new Uri(client.BaseAddress + $"api/posts/delete?postId={postId}");
            var response = await client.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<List<Post>> GetPosts()
    {
        try
        {
            var uri = new Uri(client.BaseAddress + "api/posts");
            var response = await client.GetAsync(uri);
            return JsonConvert.DeserializeObject<List<Post>>(response.Content.ToString());
        }
        catch(Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}