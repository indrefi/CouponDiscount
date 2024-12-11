using Application.Interfaces;
using Application.Services.GenerateCouponCodeService;
using Microsoft.Extensions.Logging;
using Moq;

namespace CouponDiscount.Tests
{
    public class GenerateCouponCodeTestsFixture
    {

        public Mock<ILogger<GenerateCouponCodeService>> LoggerMock { get; set; }

        public GenerateCouponCodeTestsFixture()
        {
            LoggerMock = new Mock<ILogger<GenerateCouponCodeService>>();
        }
    }
}
