// using CSForum.Core.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace CSForum.Data.Config;
//
// public class UserConfiguration:IEntityTypeConfiguration<User>
// {
//     public void Configure(EntityTypeBuilder<User> builder)
//     {
//         builder.HasIndex(prop=>prop.UserId);
//         builder.HasKey(prop=>prop.UserId);
//         
//         builder.Property(prop => prop.Email).HasMaxLength(100).IsRequired();
//         builder.Property(prop => prop.Gender);
//         builder.Property(prop => prop.Login).HasMaxLength(50).IsRequired();
//         builder.Property(prop => prop.Password).IsRequired();
//         
//     }
// }