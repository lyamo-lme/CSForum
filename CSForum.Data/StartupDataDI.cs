using CSForum.Core.IRepositories;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CSForum.Data;

public static class StartupDataDI
{
    public static IServiceCollection AddDbForumContext(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<ForumContext>(option =>
        {
            option.UseSqlServer(connectionString);
        });
        
        return serviceCollection;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        serviceCollection.AddScoped<IAnswerRepository, AnswerRepository>();
        return serviceCollection;
    }
}