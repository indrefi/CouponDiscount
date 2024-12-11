using CouponService.Validators;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Application.UseCases.DiscountCoupons.Queries.GetCoupon;
using System.Collections.Generic;

namespace CouponService.GRPCServices
{
    public class GRPCGetUnsedCouponsService : GetUnusedCouponService.GetUnusedCouponServiceBase
    {
        private readonly ILogger<GRPCGetUnsedCouponsService> _logger;
        private readonly IGetUnusedCouponsService _getUnusedCouponsService;

        public GRPCGetUnsedCouponsService(ILogger<GRPCGetUnsedCouponsService> logger, IGetUnusedCouponsService getUnusedCouponsService)
        {
            _logger = logger;
            _getUnusedCouponsService = getUnusedCouponsService;
        }

        public override async Task<GetUnusedCouponsResponse> GetCoupons(GetUnusedCouponsRequest request, ServerCallContext context)
        {
            if (request.Validate())
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancellationToken = cts.Token;

                var coupons = await _getUnusedCouponsService.GetUnusedCoupons((int)request.CouponsNumber, cancellationToken);

                var result = new GetUnusedCouponsResponse();
                result.Coupons.AddRange(coupons);
                
                return result;
            }
            else
            {
                _logger.LogInformation("Invalid Data Request", request);

                var result = new GetUnusedCouponsResponse();
                result.Coupons.AddRange(new List<string>());

                return result;
            }
        }
    }
}
