using AutoFixture;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using FluentAssertions;
using Moq;


namespace CSForum.WebApi.Test;

public class UnitOfWorkTests
{
    private readonly Mock<IUnitOfWorkRepository> _uofRepository = new Mock<IUnitOfWorkRepository>();
    private readonly Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

    [Fact]
    public async Task DbSetTest_GetAllUsers_NotNullListOfUsers()
    {
        //arrange
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var usersList = fixture.Create<List<User>>();
        
        _userRepository.Setup(x => x.GetAsync(null, null, ""))
            .ReturnsAsync(usersList);

        var users = await _userRepository.Object.GetAsync();

        //assert
        (await _userRepository.Object.GetAsync()).Should().Contain(usersList);
    }

      
    [Fact]  
    public async Task DbSetTest_AddUserEntity_UserDbSetMustContainOneEntity()
    {
        var user = CreateEntityWithoutThrowingRecursionError<User>();
        _userRepository.Setup(x => x.CreateAsync(user)).ReturnsAsync(
            user);
        var createdEntity = await _userRepository.Object.CreateAsync(user);
        createdEntity.Should().Be(user);
    }
    [Fact]  
    public async Task DbSetTest_GetUserById_MustReturnOneCreatedEntity()
    {
        var user = CreateEntityWithoutThrowingRecursionError<User>();
        _userRepository.Setup(x => x.FindAsync(x=>x.Id==user.Id)).
            ReturnsAsync(user);

        var entity = await _userRepository.Object.FindAsync(x => x.Id == user.Id);
        
        entity.Should().Be(user);
    }
    
    private T CreateEntityWithoutThrowingRecursionError<T>()
    {
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture.Create<T>();
    }
}