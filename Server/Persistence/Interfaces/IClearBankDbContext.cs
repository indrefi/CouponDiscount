using System.Threading.Tasks;
using System.Threading;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Persistence.Interfaces
{
    public interface ICouponDbContext : IBaseDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DiscountCoupon> DiscountCoupons { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}