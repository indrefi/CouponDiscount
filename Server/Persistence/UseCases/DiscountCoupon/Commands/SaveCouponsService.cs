using Application.UseCases.DiscountCoupons.Commands.SaveCoupons;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.UseCases.DiscountCoupons.Commands.SaveCoupons
{
    public class SaveCouponsService : ISaveCouponsService
    {
        private readonly ICouponDbContext _dbContext;
        private readonly ILogger<SaveCouponsService> _logger;

        public SaveCouponsService(ICouponDbContext dbContext, ILogger<SaveCouponsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> SaveBulk(List<string> coupons, CancellationToken cancellationToken)
        {
            var couponsEntities = new List<Domain.Entities.DiscountCoupon>();
            try
            {
                foreach (var coupon in coupons)
                {
                    couponsEntities.Add(new Domain.Entities.DiscountCoupon
                    {
                        CouponCode = coupon,
                        IsUsed = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    });
                }
                _dbContext.DiscountCoupons.AddRange(couponsEntities);
                var result = await _dbContext.SaveChangesAsync(cancellationToken);

                return result == coupons.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered in {this.GetType().Name} ", ex);

                return false;
            }
            
        } 
    }
}
