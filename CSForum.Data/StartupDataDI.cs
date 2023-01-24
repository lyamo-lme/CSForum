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
            option.UseSqlServer(connectionString, b=>
                b.MigrationsAssembly(assembly));
        });
        
        return serviceCollection;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IRepository<User>, UserRepository>();
        serviceCollection.AddTransient<IRepository<Post>, PostRepository>();
        serviceCollection.AddTransient<IRepository<Answer>, AnswerRepository>();
        serviceCollection.AddTransient<IRepository<Tag>, TagRepository>();
        return serviceCollection;
    }
}