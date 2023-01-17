using CSForum.Core.Models;
using CSForum.Data.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Context;

public class ForumDbContext : IdentityDbContext<User>
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {}
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddForumConfig();
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
}