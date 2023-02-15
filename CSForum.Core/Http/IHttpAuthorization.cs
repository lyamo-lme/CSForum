namespace CSForum.WebUI.Services.Interfaces;

public interface IHttpAuthorization
{
    public Task<string> GetToken(string nameRefreshToken);
    public Task UpdateTokens(Dictionary<string,string> tokens);
}