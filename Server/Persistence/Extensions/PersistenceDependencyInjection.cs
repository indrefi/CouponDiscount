using Application.UseCases.DiscountCoupons.Commands.SaveCoupons;
using Application.UseCases.DiscountCoupons.Commands.UpdateCoupons;
using Application.UseCases.DiscountCoupons.Queries.GetCoupon;
using Application.UseCases.Users.Commands.GenerateToken;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Interfaces;
using Persistence.UseCases.DiscountCoupon.Queries;
using Persistence.UseCases.DiscountCoupons.Commands.SaveCoupons;
using Persistence.UseCases.Users.Commands;

namespace Persistence.Extensions
{
    public static class PersistenceDependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(ISaveCouponsService), typeof(SaveCouponsService));
            services.AddTransient(typeof(IUpdateCouponService), typeof(UpdateCouponService));
            services.AddTransient(typeof(IGetUnusedCouponsService), typeof(GetUnusedCouponsService));
            services.AddTransient(typeof(IGenerateUserTokenService), typeof(GenerateUserTokenService));

            services.AddDBContext(configuration);

            return services;
        }

        public static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CouponDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("CouponDatabase");

                options.UseSqlServer(connectionString);
            });

            services.AddScoped<ICouponDbContext>(provider => provider.GetRequiredService<CouponDbContext>());

            return services;
        }
    }
}