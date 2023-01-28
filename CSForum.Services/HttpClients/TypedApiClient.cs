using CSForum.Shared.Models;
using Microsoft.Extensions.Options;

namespace CSForum.Services.HttpClients;

public abstract class  TypedApiClient
{
    public readonly HttpClient client;
    private readonly ApiSettingConfig _apiSettings;
    public TypedApiClient(HttpClient client, ApiSettingConfig apiSettings)
    {
        this.client = client;
        this._apiSettings = apiSettings;
        this.client.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }
    public TypedApiClient(ApiSettingConfig apiSettings)
    {
        client = new HttpClient();
        _apiSettings = apiSettings;
        client.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }
}