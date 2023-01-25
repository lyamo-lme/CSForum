namespace CSForum.Core.IHttpClients;

public interface IHttpClient<T>
{
    public Task<T> CreateAsync<TDto>(TDto model);
    public Task<T> EditAsync<TDto>(TDto model);
    public Task<bool> DeleteAsync(int postId);
    public Task<List<T>> GetAsync();
    public Task<T> FindAsync(int id);
}