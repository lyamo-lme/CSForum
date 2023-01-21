using CSForum.Shared.Models;
using Microsoft.Extensions.Options;

namespace CSForum.Services.HttpClients;

public abstract class  TypedApiClient
{
    public readonly HttpClient client;
    private readonly ApiSettingConfig apiSettings;
    public TypedApiClient(HttpClient client, IOptions<ApiSettingConfig> apiSettings)
    {
        this.client = client;
        this.apiSettings = apiSettings.Value;
        this.client.BaseAddress = new Uri(this.apiSettings.webApiUrl);
    }
}