using Microsoft.EntityFrameworkCore;

namespace CSForum.Data.Config;

public static class ConfigExtensions
{
    public static void AddDbForumConfig(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnswerConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PostTagsConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        modelBuilder.ApplyConfiguration(new UsersChatsConfiguration());
        // modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}