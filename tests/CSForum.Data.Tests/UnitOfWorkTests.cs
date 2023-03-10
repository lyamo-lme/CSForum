using CSForum.Core.Models;
using CSForum.Data.Tests.Helpers;
using FluentAssertions;

namespace CSForum.Data.Tests;

public class UnitOfWorkTests
{
    private readonly UnitOfWorkFixture _fixture;

    public UnitOfWorkTests()
    {
        _fixture = new UnitOfWorkFixture();
    }


    [Fact]
    public void CheckNotNullInstance()
    {
        var sut = _fixture.Create();

        Assert.NotNull(sut);
    }

    [Fact]
    public async Task Repository_GetPosts_ShouldHavePosts()
    {
        var sut = _fixture.Create();
        var repository = sut.GenericRepository<Post>();

        var posts = (await sut.GenericRepository<Post>().GetAsync());

        posts.Should().NotBeNull();
        posts.Count().Should().BePositive();
    }

    [Fact]
    public async Task Repository_GetUsers_ListOfUsers()
    {
        var sut = _fixture.Create();
        var repository = sut.GenericRepository<User>();

        var users = await repository.GetAsync();
        var list = users.ToList();
        list.Should().HaveCount(UnitOfWorkHelper.SeedUsers().Count());
    }

    [Fact]
    public async Task Repository_FindUser_FindUserWithId_1()
    {
        var sut = _fixture.Create();
        var repository = sut.GenericRepository<User>();

        var user = await repository.FindAsync(x => x.Id == 1);
        user.Should().NotBeNull();
        user.Id.Equals(1);
    }
}