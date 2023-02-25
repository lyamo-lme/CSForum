using CSForum.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSForum.Data.Config;

public class UsersChatsConfiguration:IEntityTypeConfiguration<UsersChats>
{
    public void Configure(EntityTypeBuilder<UsersChats> builder)
    {
        builder.HasKey(prop => prop.Id);
         //fields
        builder.Property(prop => prop.UserId).IsRequired();
        builder.Property(prop => prop.ChatId).IsRequired();
        
        builder.HasOne<User>(prop => prop.User)
            .WithMany(prop => prop.UsersChats)
            .HasForeignKey(prop => prop.UserId);
        
        builder.HasOne<Chat>(prop => prop.Chat)
            .WithMany(prop => prop.UsersChats)
            .HasForeignKey(prop => prop.ChatId);
    }
}