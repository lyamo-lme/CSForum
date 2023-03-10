using AutoFixture;
using Bogus;
using Bogus.DataSets;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using Moq;
using TestLibrary;

namespace CSForum.Data.Tests.Helpers;

public static class UnitOfWorkHelper
{
    public static Mock<IUnitOfWorkRepository> GetMock()
    {
        var context = DbContextHelper.Context;
        SeedData(context);
        var uow = new Mock<IUnitOfWorkRepository>();
        uow.Setup(x => x.GenericRepository<Post>()).Returns(new GenericRepository<Post>(context));
        uow.Setup(x => x.GenericRepository<Message>()).Returns(new GenericRepository<Message>(context));
        uow.Setup(x => x.GenericRepository<User>()).Returns(new GenericRepository<User>(context));
        return uow;
    }

    public static void SeedData(ForumDbContext context)
    {
        context.AddRange(SeedUsers());
        context.AddRange(SeedPosts());
        context.SaveChanges();
    }

    public static List<Post> SeedPosts()
    {
        var postId = 1;
        var testPosts = new Faker<Post>()
            .CustomInstantiator(f => new Post()
            {
                Id = postId++
            })
            .RuleFor(u => u.Content, (f, u) => f.Random.Words())
            .RuleFor(u => u.UserId, (f, u) => u.Id)
            .RuleFor(u => u.Title, (f, u) => f.Random.Word())
            .RuleFor(u => u.DateCreate, (f, u) => DateTime.Now);

        return testPosts.Generate(10);
    }

    public static List<User> SeedUsers()
    {
        var userIds = 1;
        var testUsers = new Faker<User>()
            .CustomInstantiator(f => new User()
            {
                Id = userIds++
            })
            .RuleFor(u => u.UserName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.Email, (f, u) => f.Person.Email)
            .RuleFor(u => u.PhoneNumber, (f, u) => f.Phone.PhoneNumber())
            .RuleFor(u => u.NormalizedEmail, (f, u) => f.Person.Email.ToUpper())
            .RuleFor(u => u.PasswordHash, (f, u) => f.Random.Hash())
            .RuleFor(u => u.NormalizedUserName, (f, u) => f.Name.FirstName().ToUpper());

        return testUsers.Generate(10);
    }
}