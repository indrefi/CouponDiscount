using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.DiscountCoupons.Commands.UpdateCoupons
{
    public interface IUpdateCouponService
    {
        public Task<int> UpdateCoupon(string couponCode, CancellationToken cancellationToken);
    }
}
