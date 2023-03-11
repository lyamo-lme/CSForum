using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Tests;

public static class DbContextHelper
{
    public static ForumDbContext? Context;

    static DbContextHelper()
    {
        var builder = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: "f");
        builder = builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        Context = new ForumDbContext(builder.Options);
    }
}