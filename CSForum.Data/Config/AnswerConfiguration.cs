using CSForum.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSForum.Data.Config;

public class AnswerConfiguration:IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.HasIndex(prop => prop.Id);

        builder.Property(prop=>prop.PostId).IsRequired();
        builder.Property(prop=>prop.UserId).IsRequired();
        builder.Property(prop=>prop.DateCreate).IsRequired();
        builder.Property(prop => prop.ContentBody).IsRequired();

        builder.HasOne<User>(prop => prop.AnswerCreator)
            .WithMany(prop => prop.Answers)
            .HasForeignKey(p=>p.UserId);
        
        builder.HasOne<Post>(prop => prop.Post)
            .WithMany(prop => prop.Answers)
            .HasForeignKey(p=>p.PostId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}