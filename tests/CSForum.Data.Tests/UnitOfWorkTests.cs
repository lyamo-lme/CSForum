using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Tests.Helpers;
using FluentAssertions;

namespace CSForum.Data.Tests;

public class UnitOfWorkTests
{
    private readonly UnitOfWorkFixture? _fixture = null;
    private IUnitOfWorkRepository? uof = null;

    public UnitOfWorkTests()
    {
        _fixture = new UnitOfWorkFixture();

        uof = _fixture.Create();
    }

    [Fact]
    public void CheckNotNullInstance()
    {
        Assert.NotNull(uof);
    }

    [Fact]
    public async Task Repository_GetPosts_ShouldHavePosts()
    {
        var posts = (await uof.GenericRepository<Post>().GetAsync()).ToList();

        posts.Should().NotBeNull();
        posts.Count().Should().BePositive();
    }

    [Fact]
    public async Task Repository_GetUsers_ListOfUsers()
    {
        var repository = uof.GenericRepository<User>();
        var users = await repository.GetAsync();
        var list = users.ToList();
        list.Should().HaveCount(UnitOfWorkHelper.SeedUsers().Count());
    }

    [Fact]
    public async Task Repository_FindUser_FindUserWithId_1()
    {
        var repository = uof.GenericRepository<User>();

        var user = await repository.FindAsync(x => x.Id == 1);
        user.Should().NotBeNull();
        user.Id.Equals(1);
    }
}