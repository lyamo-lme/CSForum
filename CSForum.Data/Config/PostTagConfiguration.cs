using CSForum.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSForum.Data.Config;

public class PostTagsConfiguration:IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder.HasKey(pt => new 
        {
            pt.PostId,
            pt.TagId
        });
        
        builder.HasOne(
                p => p.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(p=>p.PostId);
        
        builder.HasOne(
                p => p.Tag)
            .WithMany(p => p.TagPosts)
            .HasForeignKey(p=>p.TagId);
    }
}