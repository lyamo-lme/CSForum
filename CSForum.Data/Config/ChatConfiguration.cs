// using CSForum.Core.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace CSForum.Data.Config;
//
// public class ChatConfiguration:IEntityTypeConfiguration<Chat>
// {
//     public void Configure(EntityTypeBuilder<Chat> builder)
//     {
//         builder.HasIndex(prop => prop.ChatId);
//         builder.HasKey(prop=>prop.ChatId);
//
//         builder.Property(prop=>prop.FirstUserId).IsRequired();
//         builder.Property(prop=>prop.SecondUserId).IsRequired();
//
//         builder.HasOne<User>(prop => prop.FirstUser)
//             .WithMany(prop => prop.)
//             .HasForeignKey(p=>p.UserId);
//         
//         builder.HasOne<Post>(prop => prop.Post)
//             .WithMany(prop => prop.Answers)
//             .HasForeignKey(p=>p.PostId)
//             .OnDelete(DeleteBehavior.NoAction);
//
//     }
// }