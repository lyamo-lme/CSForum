using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CSForum.Data;

public static class StartupDataDI
{
    public static IServiceCollection AddDbForumContext(this IServiceCollection serviceCollection, string connectionString, string assembly)
    {
        serviceCollection.AddDbContext<ForumDbContext>(option =>
        {
            option.UseSqlServer(connectionString);
        });
        
        return serviceCollection;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IUserRepository, UserRepository>();
        serviceCollection.AddTransient<IPostRepository, PostRepository>();
        serviceCollection.AddTransient<IAnswerRepository, AnswerRepository>();
        serviceCollection.AddTransient<ITagRepository, TagRepository>();
        return serviceCollection;
    }
}