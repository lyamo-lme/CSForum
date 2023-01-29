namespace CSForum.Shared.Models;

public class ApiSettingConfig
{
	public string WebApiUrl { get; set; }
	public string IdentityServerUrl { get; set; }

	public ApiSettingConfig(string webApi, string identityServerUrl)
	{
		WebApiUrl = webApi;
		this.IdentityServerUrl = identityServerUrl;
	}
	public ApiSettingConfig()
	{ }
}