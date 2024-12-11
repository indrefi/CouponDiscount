using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class DiscountCouponConfiguration : IEntityTypeConfiguration<DiscountCoupon>
    {
        public void Configure(EntityTypeBuilder<DiscountCoupon> builder)
        {
            builder.ToTable(name: "DiscountCoupon", schema: "Coupon")
                .HasKey(dc => new { dc.DiscountCouponID });

            builder.Property(e => e.CouponCode)
                   .HasColumnName("CouponCode")
                   .HasColumnType("VARCHAR")
                   .HasMaxLength(8)
                   .IsRequired();

            builder.Property(e => e.CreatedAt)
                   .HasColumnName("CreatedAt")
                   .HasColumnType("DATETIME")
                   .IsRequired();

            builder.Property(e => e.UpdatedAt)
                   .HasColumnName("UpdatedAt")
                   .HasColumnType("DATETIME")
                   .IsRequired();

            builder.Property(e => e.IsUsed)
                   .HasColumnName("IsUsed")
                   .HasColumnType("BIT")
                   .IsRequired();

        }
    }
}
