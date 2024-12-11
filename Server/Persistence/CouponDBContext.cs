using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Domain.Entities;
using Persistence.Interfaces;

namespace Persistence
{
    public class CouponDbContext : DbContext, ICouponDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DiscountCoupon> DiscountCoupons { get; set; }

        public CouponDbContext(DbContextOptions<CouponDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CouponDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}