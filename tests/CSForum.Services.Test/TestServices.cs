using AutoFixture;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using CSForum.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TestLibrary;

namespace CSForum.Services.Test;

public class TestServices
{
    private readonly Mock<IChatService> _chatService = new Mock<IChatService>();
    private readonly Fixture _fixture = new Fixture();
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public TestServices()
    {
        DbContextOptionsBuilder<ForumDbContext> options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: "f");
        ForumDbContext d = new ForumDbContext(options.Options);
        _unitOfWorkRepository = new UowRepository(new RepositoryFactory(),d);
    }
    
    [Fact]
    public async Task d()
    {
        var entity = _fixture.CreateEntityWithoutThrowingRecursionError<Post>();
        
        var post = await _unitOfWorkRepository.GenericRepository<Post>().CreateAsync(entity);
        await _unitOfWorkRepository.SaveAsync();

        post.Should().Be(entity);
    }

    [Fact]
    public async Task ChatService_AddMessage_ReturnNewMessage()
    {
        var message = _fixture.CreateEntityWithoutThrowingRecursionError<Message>();

        _chatService.Setup(x => x.AddMessageAsync(message, 1)).ReturnsAsync(message);

        var messageEntity = await _chatService.Object.AddMessageAsync(message, 1);

        messageEntity.Should().Be(message);
    }
}