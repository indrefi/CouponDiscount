using Application.Interfaces;
using Application.Services.GenerateCouponCodeService;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenerateCouponCodeService), typeof(GenerateCouponCodeService));

            return services;
        }
    }
}
