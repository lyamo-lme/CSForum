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
        });
        
        return serviceCollection;
    }
    public static IServiceCollection AddForumDbContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IRepository<Tag>, GenericRepository<Tag>>();
        serviceCollection.AddTransient<IRepository<PostTag>, GenericRepository<PostTag>>();
        serviceCollection.AddTransient<IRepository<User>, GenericRepository<User>>();
        serviceCollection.AddTransient<IRepository<Post>, GenericRepository<Post>>();
        serviceCollection.AddTransient<IUnitOfWorkRepository, UOWRepository>();
        return serviceCollection;
    }
}