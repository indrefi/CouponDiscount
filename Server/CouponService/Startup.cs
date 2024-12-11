using Application.Common;
using Application.Extensions;
using CouponService.GRPCServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistence.Extensions;
using ProtoBuf.Meta;
using System.Text;

namespace CouponService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CryptoSettingsOptions>(Configuration.GetSection("CryptoSettingsOptions"));

            var symetricKey = Configuration["CryptoSettingsOptions:SymetricJWTKey"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AuthTokenConstants.ISSUER,
                    ValidAudience = AuthTokenConstants.AUDIENCE,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symetricKey))
                };
            });
            services.AddAuthorization(options =>
            {
                // TODO: Add policies if needed or claim checks.
            });

            services.AddGrpc();
            services.AddApplication();
            services.AddPersistence(Configuration);          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GRPCGenerateCouponService>().RequireAuthorization();
                endpoints.MapGrpcService<GRPCUseCouponService>().RequireAuthorization();
                endpoints.MapGrpcService<GRPCGetUnsedCouponsService>().RequireAuthorization();
                endpoints.MapGrpcService<GRPCGetTokenService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
