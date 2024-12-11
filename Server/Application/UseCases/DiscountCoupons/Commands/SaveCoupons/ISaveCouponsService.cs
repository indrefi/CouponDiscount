using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.DiscountCoupons.Commands.SaveCoupons
{
    public interface ISaveCouponsService
    {
        Task<bool> SaveBulk(List<string> coupons, CancellationToken cancellationToken);
    }
}
