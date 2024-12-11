using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(name: "User", schema: "User")
                .HasKey(u => new { u.UserID });

            builder.Property(e => e.UserName)
                   .HasColumnName("UserName")
                   .HasColumnType("VARCHAR")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.UserPassword)
                   .HasColumnName("UserPassword")
                   .HasColumnType("VARCHAR")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.UpdatedAt)
                   .HasColumnName("UpdatedAt")
                   .HasColumnType("DATETIME")
                   .IsRequired();

        }
    }
}
