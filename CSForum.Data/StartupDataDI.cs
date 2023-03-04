using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Context;
using CSForum.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CSForum.Data;

public static class StartupDataDi
{
    public static IServiceCollection AddDbForumContext(this IServiceCollection serviceCollection, string connectionString, string assembly)
    {
        serviceCollection.AddDbContext<ForumDbContext>(option =>
        {
            option.UseSqlServer(connectionString, b=>
                b.MigrationsAssembly(assembly));
             option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        
        return serviceCollection;
    }
    public static IServiceCollection AddForumDbContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRepository<Tag>, GenericRepository<Tag>>();
        serviceCollection.AddScoped<IRepository<PostTag>, GenericRepository<PostTag>>();
        serviceCollection.AddScoped<IRepository<User>, GenericRepository<User>>();
        serviceCollection.AddScoped<IRepository<Post>, GenericRepository<Post>>();
        serviceCollection.AddScoped<IRepository<Chat>, GenericRepository<Chat>>();
        serviceCollection.AddScoped<IRepository<Message>, GenericRepository<Message>>();
        serviceCollection.AddScoped<IUnitOfWorkRepository, UowRepository>();
        return serviceCollection;
    }
}