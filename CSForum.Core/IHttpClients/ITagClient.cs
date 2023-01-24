using CSForum.Core.Models;

namespace CSForum.Core.IHttpClients;

public interface ITagClient
{
    public Task<Tag> CreateTag(Tag model);
    public Task<Tag> EditTag(Tag model);
    public Task<bool> DeleteTag(int id);
    public Task<List<Tag>> GetTag();
}