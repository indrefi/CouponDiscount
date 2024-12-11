using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using Application.UseCases.Users.Commands.GenerateToken;
using System.Threading.Tasks;
using Application.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Application.Common;
using Microsoft.Extensions.Options;

namespace Persistence.UseCases.Users.Commands
{
    public class GenerateUserTokenService : IGenerateUserTokenService
    {
        private readonly ICouponDbContext _dbContext;
        private readonly ILogger<GenerateUserTokenService> _logger;
        private readonly CryptoSettingsOptions _cryptoSettingsOptions;


        public GenerateUserTokenService(ICouponDbContext dbContext, ILogger<GenerateUserTokenService> logger, IOptions<CryptoSettingsOptions> cryptoSettingsOptions)
        {
            _dbContext = dbContext;
            _logger = logger;
            _cryptoSettingsOptions = cryptoSettingsOptions.Value;
        }

        public async Task<string> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);
                var passwordHash = CryptoExtension.CalculateSHA1Hash(request.UserPassword);

                if (user != null && passwordHash == user.UserPassword)
                {
                    var token = GenerateJwtToken(user.UserName);
                    return token;
                }
                else
                {
                    _logger.LogInformation($"Invalid Credentials for user: {request.UserName}");
                }

                return string.Empty;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error encountered in {this.GetType().Name} ", ex);

                return string.Empty;
            }
        }

        private string GenerateJwtToken(string username)
        {
            var key = _cryptoSettingsOptions.SymetricJWTKey;
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, AuthTokenConstants.SUBJECT),
                new Claim(JwtRegisteredClaimNames.Iss, AuthTokenConstants.ISSUER),
                new Claim(JwtRegisteredClaimNames.Aud, AuthTokenConstants.AUDIENCE),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(30).ToString())
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: AuthTokenConstants.ISSUER,
                audience: AuthTokenConstants.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
