using System.Linq.Expressions;
using AutoFixture;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using CSForum.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestLibrary;

namespace CSForum.Data.Tests;

public class DataTests
{
    private readonly Fixture _fixture = new Fixture();
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private ForumDbContext d;
    public DataTests()
    {
        DbContextOptionsBuilder<ForumDbContext> options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: "f");
       new ForumDbContext(options.Options);
        _unitOfWorkRepository = new UowRepository(new RepositoryFactory(), d);
    }

    [Fact]
    public async Task AddEntity_ShouldReturnEntity()
    {
        var entity = _fixture.CreateEntityWithoutThrowingRecursionError<Post>();

        var post = await _unitOfWorkRepository.GenericRepository<Post>().CreateAsync(entity);
        await _unitOfWorkRepository.SaveAsync();

        post.Should().Be(entity);
    }

}