using Application.Common;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.DiscountCoupons.Commands.UpdateCoupons
{
    public class UpdateCouponService : IUpdateCouponService
    {
        private readonly ICouponDbContext _dbContext;
        private readonly ILogger<UpdateCouponService> _logger;

        public UpdateCouponService(ICouponDbContext dbContext, ILogger<UpdateCouponService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> UpdateCoupon(string couponCode,CancellationToken cancellationToken)
        {
            try
            {
                var couponEntity = _dbContext.DiscountCoupons.Where(x => x.CouponCode.Equals(couponCode)).FirstOrDefault();
                if (couponCode == null)
                {
                    return (int)UseCodeResponseCodes.NotFound;
                }
                else
                {
                    if (couponEntity.IsUsed) return (int)UseCodeResponseCodes.AlreadyUsed;

                    couponEntity.IsUsed = true;
                    _dbContext.DiscountCoupons.Update(couponEntity);
                    var result = await _dbContext.SaveChangesAsync(cancellationToken);

                    return result == 1 ? (int)UseCodeResponseCodes.Success : (int)UseCodeResponseCodes.Error;
                }
            }
            catch(Exception ex) 
            {
                _logger.LogError($"Error encountered in {this.GetType().Name} ", ex);

                return (int)UseCodeResponseCodes.Error;
            }          
            
        }
    }
}
