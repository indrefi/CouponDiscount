using Application.Services.GenerateCouponCodeService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenerateCouponCodeService
    {
        Task<List<string>> GenerateCouponCodes(GenerateCouponCodeRequest generateCouponCodeRequest);
    }
}
