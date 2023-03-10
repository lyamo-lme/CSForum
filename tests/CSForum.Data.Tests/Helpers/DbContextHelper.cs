using CSForum.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Tests;

public static class DbContextHelper
{
    public static ForumDbContext Context;

    static DbContextHelper()
    {
        var builder = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: "f");
        Context = new ForumDbContext(builder.Options);
    }
}