using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CSForum.Data;

public static class StartupDI
{
    public static IServiceCollection AddDbForumContext(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<ForumContext>(option =>
        {
            option.UseSqlServer(connectionString);
        });
        return serviceCollection;
    }
}