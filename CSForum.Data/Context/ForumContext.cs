using CSForum.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Context;

public class ForumContext:DbContext
{
    public ForumContext(DbContextOptions<ForumContext> options):base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Answer> Answers { get; set; }
}