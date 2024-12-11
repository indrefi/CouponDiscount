using Application.UseCases.DiscountCoupons.Queries.GetCoupon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.UseCases.DiscountCoupon.Queries
{
    public class GetUnusedCouponsService : IGetUnusedCouponsService
    {
        private readonly ICouponDbContext _dbContext;
        private readonly ILogger<GetUnusedCouponsService> _logger;

        public GetUnusedCouponsService(ICouponDbContext dbContext, ILogger<GetUnusedCouponsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<List<string>> GetUnusedCoupons(int couponsNumber, CancellationToken cancellationToken)
        {
            try
            {
                var entities = _dbContext.DiscountCoupons.Where(x => x.IsUsed == false).Select(x => x.CouponCode).Take(couponsNumber);
                if (entities != null && entities.Any()) 
                {
                    return entities.ToListAsync(cancellationToken);
                }
                else
                {
                    _logger.LogWarning($"No unused coupons available. Requested {couponsNumber}");

                    return Task.FromResult(new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered in {this.GetType().Name} ", ex);

                return Task.FromResult(new List<string>());
            }
        }
    }
}
