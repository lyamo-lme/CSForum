using CSForum.Core.Models;
using CSForum.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Context;

public class ForumContext : DbContext
{
    public ForumContext(DbContextOptions<ForumContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostTag>().HasKey(pt => new 
        {
            pt.PostId,
            pt.TagId
        });
        
        modelBuilder.ApplyConfiguration( new AnswerConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
}