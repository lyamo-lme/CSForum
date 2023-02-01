using AutoFixture;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using Moq;

namespace CSForum.WebApi.Test;

public class UnitOfWorkTests
{
    private readonly Mock<IUnitOfWorkRepository> _uofRepository = new Mock<IUnitOfWorkRepository>();
    private readonly Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

    [Fact]
    public async Task UserDbSetTest_GetAllUsers_NotNullListOfUsers()
    {
        //arrange
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var usersList = fixture.Create<List<User>>();
        
        
        _userRepository.Setup(x => x.GetAsync(null, null, ""))
            .ReturnsAsync(usersList);
        
         var users = await _userRepository.Object.GetAsync();

        //asert
        Assert.Equal(users, usersList);
    }
}