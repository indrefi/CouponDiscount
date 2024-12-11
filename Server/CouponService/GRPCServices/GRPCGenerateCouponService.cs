using Application.Interfaces;
using Application.Services.GenerateCouponCodeService;
using Application.UseCases.DiscountCoupons.Commands.SaveCoupons;
using CouponService.Validators;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CouponService
{
    public class GRPCGenerateCouponService : GenerateDiscountCouponService.GenerateDiscountCouponServiceBase
    {
        private readonly ILogger<GRPCGenerateCouponService> _logger;
        private readonly IGenerateCouponCodeService _generateCouponCodeService;
        private readonly ISaveCouponsService _saveCouponsService;

        public GRPCGenerateCouponService(ILogger<GRPCGenerateCouponService> logger, IGenerateCouponCodeService generateCouponCodeService, ISaveCouponsService saveCouponsService)
        {
            _logger = logger;
            _generateCouponCodeService = generateCouponCodeService;
            _saveCouponsService = saveCouponsService;
        }

        public override async Task<GenerateResponse> Generate(GenerateRequest request, ServerCallContext context)
        {
            if (request.Validate()) 
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancellationToken = cts.Token;

                var couponCodes = await _generateCouponCodeService.GenerateCouponCodes(new GenerateCouponCodeRequest { CouponInstances = (int)request.Count, CouponLength = request.Length });
                if(couponCodes.Count != request.Count) return new GenerateResponse { Result = true };

                // TODO: The insert can also fail due to UQ violation. But in this case it will return false for bulk insert.
                // Here we can eventually add a retry mechanism on generate+savebulk and extract this to a private method or to a separate service.
                var saveResult = await _saveCouponsService.SaveBulk(couponCodes, cancellationToken);
                var result = new GenerateResponse
                {
                    Result = saveResult,
                };

                return result;
            }
            else
            {
                _logger.LogInformation("Invalid Data Request", request);

                return new GenerateResponse
                {
                    Result = false,
                };
            }            
        }
    }
}
