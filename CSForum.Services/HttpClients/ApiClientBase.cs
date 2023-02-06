using CSForum.Shared.Models;
using Microsoft.Extensions.Options;

namespace CSForum.Services.HttpClients;

public abstract class  ApiClientBase
{
    public readonly HttpClient client;
    private readonly ApiSettingConfig _apiSettings;
    public ApiClientBase(HttpClient client, ApiSettingConfig apiSettings)
    {
        this.client = client;
        this._apiSettings = apiSettings;
        this.client.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }
    public ApiClientBase(ApiSettingConfig apiSettings)
    {
        client = new HttpClient();
        _apiSettings = apiSettings;
        client.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }
}