using CSForum.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSForum.Data.Config;

public class PostConfiguration:IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasIndex(prop=>prop.Id);
        builder.HasKey(prop=>prop.Id);
        
        builder.HasOne<User>(prop => prop.PostCreator)
            .WithMany(prop=>prop.Posts)
            .HasForeignKey(prop=>prop.UserId)
            .HasConstraintName("FK_Post_User");
        
        
        //fields
        builder.Property(prop => prop.UserId).IsRequired();
        builder.Property(prop => prop.DateCreate).IsRequired();
        builder.Property(prop => prop.Title).IsRequired();
        
    }
}