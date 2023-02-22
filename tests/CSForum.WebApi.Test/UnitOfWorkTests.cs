using AutoFixture;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using FluentAssertions;
using Moq;
using TestLibrary;


namespace CSForum.WebApi.Test;

public class UnitOfWorkTests
{
    private readonly Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task RepositoryTest_GetAllUsers_NotNullListOfUsers()
    {
        //arrange
         var usersList = _fixture.CreateEntityWithoutThrowingRecursionError<List<User>>();
         
        _userRepository.Setup(x => x.GetAsync(null, null, null,null,""))
            .ReturnsAsync(usersList);

        var users = await _userRepository.Object.GetAsync();

        //assert
        (await _userRepository.Object.GetAsync()).Should().Contain(usersList);
    }

      
    [Fact]  
    public async Task RepositoryTest_AddUserEntity_UserDbSetMustContainOneEntity()
    {
        var user = _fixture.CreateEntityWithoutThrowingRecursionError<User>();

        _userRepository.Setup(x => x.CreateAsync(user)).ReturnsAsync(
            user);

        var createdEntity = await _userRepository.Object.CreateAsync(user);

        createdEntity.Should().Be(user);
    }
    [Fact]  
    public async Task RepositoryTest_GetUserById_MustReturnOneCreatedEntity()
    {
        var user =  _fixture.CreateEntityWithoutThrowingRecursionError<User>();
        _userRepository.Setup(x => x.FindAsync(x=>x.Id==user.Id)).
            ReturnsAsync(user);

        var entity = await _userRepository.Object.FindAsync(x => x.Id == user.Id);
        
        entity.Should().Be(user);
    }
 
}