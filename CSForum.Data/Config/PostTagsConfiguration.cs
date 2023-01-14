using CSForum.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSForum.Data.Config;

public class PostTagsConfiguration:IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        
        builder.HasOne<Post>(
                p => p.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(p=>p.PostId);
        
        builder.HasOne<Tag>(
                p => p.Tag)
            .WithMany(p => p.TagPosts)
            .HasForeignKey(p=>p.TagId);
    }
}