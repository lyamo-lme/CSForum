using CSForum.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CSForum.Services.Http;

public abstract class  ApiClientBase
{
    protected readonly HttpClient hc;
    private readonly ApiSettingConfig? _apiSettings;

    protected ApiClientBase(HttpClient hc, ApiSettingConfig apiSettings)
    {
        this.hc = hc;
        this._apiSettings = apiSettings;
        this.hc.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }

    protected ApiClientBase(ApiSettingConfig apiSettings)
    {
        hc = new HttpClient();
        _apiSettings = apiSettings;
        hc.BaseAddress = new Uri(this._apiSettings.WebApiUrl);
    }
    

    protected ApiClientBase()
    {
        hc = new HttpClient();
        _apiSettings = null;
        
    }
}