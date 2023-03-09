namespace CSForum.Core.Http;

public interface IHttpAuthorization
{
    public Task<string> GetToken(string nameRefreshToken);
    public Task UpdateTokens(Dictionary<string,string> tokens);
}