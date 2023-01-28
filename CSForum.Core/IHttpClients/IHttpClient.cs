namespace CSForum.Core.IHttpClients;

public interface IHttpClient
{
    public Task<TOut> PostAsync<TDto, TOut>(TDto model, string? path = null);
    public Task<TDto> GetAsync<TDto>(string path);
    public Task<TDto> PostAsync<TDto>(TDto model, string? path = null);

}