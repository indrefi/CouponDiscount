using Application.Common;
using Application.UseCases.DiscountCoupons.Commands.UpdateCoupons;
using CouponService.Validators;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CouponService
{
    public class GRPCUseCouponService : UseDiscountCouponService.UseDiscountCouponServiceBase
    {
        private readonly ILogger<GRPCGenerateCouponService> _logger;
        private readonly IUpdateCouponService _updateCouponService;

        public GRPCUseCouponService(ILogger<GRPCGenerateCouponService> logger, IUpdateCouponService updateCouponService)
        {
            _logger = logger;
            _updateCouponService = updateCouponService;
        }

        public override async Task<UseCodeResponse> UseCoupon(UseCodeRequest request, ServerCallContext context)
        {
            var result = new UseCodeResponse
            {
                Result = (int)UseCodeResponseCodes.Success
            };

            if (request.Validate()) 
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancellationToken = cts.Token;

                var saveResult = await _updateCouponService.UpdateCoupon(request.Code, cancellationToken);

                result.Result = (uint)saveResult;

                return result;
            }
            else
            {
                _logger.LogInformation("Invalid Data Request", request);

                return new UseCodeResponse
                {
                    Result = (int)UseCodeResponseCodes.InvalidData
                };
            }           

        }
    }
}
