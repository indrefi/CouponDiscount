using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.DiscountCoupons.Queries.GetCoupon
{
    public interface IGetUnusedCouponsService
    {
        public Task<List<string>> GetUnusedCoupons(int couponsNumber, CancellationToken cancellationToken);
    }
}
