using CouponService.Validators;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Application.UseCases.Users.Commands.GenerateToken;

namespace CouponService.GRPCServices
{
    public class GRPCGetTokenService : GetTokenService.GetTokenServiceBase
    {
        private readonly ILogger<GRPCGenerateCouponService> _logger;
        private readonly IGenerateUserTokenService _generateUserTokenService;

        public GRPCGetTokenService(ILogger<GRPCGenerateCouponService> logger, IGenerateUserTokenService generateUserTokenService)
        {
            _logger = logger;
            _generateUserTokenService = generateUserTokenService;
        }

        public override async Task<GetTokenResponse> GetToken(GetTokenRequest request, ServerCallContext context)
        {
            if (request.Validate())
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancellationToken = cts.Token;

                var token = await _generateUserTokenService.GenerateTokenAsync(new GenerateTokenRequest { UserName = request.UserName, UserPassword = request.UserPassword }, cancellationToken);

                var result = new GetTokenResponse
                {
                    Token = token,
                };

                return result;
            }
            else
            {
                _logger.LogInformation("Invalid Data Request", request);

                return new GetTokenResponse
                {
                    Token = string.Empty,
                };
            }
        }
    }
}
