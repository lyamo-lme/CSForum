using CSForum.Shared.Models;
using Microsoft.Extensions.Options;

namespace CSForum.Services.HttpClients;

public abstract class  ApiClientBase
{
    protected readonly HttpClient client;
    private readonly ApiSettingConfig? _apiSettings;

    protected ApiClientBase(HttpClient client, ApiSettingConfig apiSettings)
    {
        this.client = client;
        this._apiSettings = apiSettings;
        this.client.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }

    protected ApiClientBase(ApiSettingConfig apiSettings)
    {
        client = new HttpClient();
        _apiSettings = apiSettings;
        client.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }
    

    protected ApiClientBase()
    {
        client = new HttpClient();
        _apiSettings = null;
        
    }
}