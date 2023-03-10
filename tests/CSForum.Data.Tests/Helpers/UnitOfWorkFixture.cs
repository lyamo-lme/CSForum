using CSForum.Core.IRepositories;

namespace CSForum.Data.Tests.Helpers;


public class UnitOfWorkFixture
{
    public IUnitOfWorkRepository Create()
    {
        var mock = UnitOfWorkHelper.GetMock();
        return mock.Object;
    }
}