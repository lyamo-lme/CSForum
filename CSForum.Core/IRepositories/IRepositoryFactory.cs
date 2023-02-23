namespace CSForum.Core.IRepositories;

public interface IRepositoryFactory
{
    public IRepository<T> Instance<T>(object dbContext) where T:class;
}