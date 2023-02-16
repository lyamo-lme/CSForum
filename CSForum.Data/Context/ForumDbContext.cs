using CSForum.Core.Models;
using CSForum.Data.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Context;

public class ForumDbContext :
    IdentityDbContext<User,IdentityRole<int>,int>
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
         Database.EnsureDeleted();
         Database.EnsureCreated();  
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.AddDbForumConfig();
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UsersChats> UsersChats { get; set; }
}