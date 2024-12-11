using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Commands.GenerateToken
{
    public interface IGenerateUserTokenService
    {
        public Task<string> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken);
    }
}