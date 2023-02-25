using CSForum.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSForum.Data.Config;

public class MessageConfiguration:IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(prop => prop.Id);


        builder.Property(prop=>prop.Content).IsRequired()
            .HasMaxLength(100);
        builder.Property(prop=>prop.UserId).IsRequired();

        builder.HasOne<Chat>(prop => prop.Chat)
            .WithMany(prop => prop.Messages)
            .HasForeignKey(p=>p.ChatId);
        
        builder.HasOne<User>(prop => prop.User)
            .WithMany(prop => prop.Messages)
            .HasForeignKey(p=>p.UserId);
     
    }
}