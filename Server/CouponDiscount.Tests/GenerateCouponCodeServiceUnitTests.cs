using Application.Services.GenerateCouponCodeService;
using System.Linq;
using Xunit;

namespace CouponDiscount.Tests
{

    public class GenerateCouponCodeServiceUnitTests : IClassFixture<GenerateCouponCodeTestsFixture>
    {
        private readonly GenerateCouponCodeTestsFixture _fixture;

        public GenerateCouponCodeServiceUnitTests(GenerateCouponCodeTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Generate_3_Coupons_Length_7_ExpectTrue()
        {
            var mockedLogger = _fixture.LoggerMock;
               
            var request3Coupons7Length = new GenerateCouponCodeRequest
            {
                CouponInstances = 3,
                CouponLength = 7,
            };

            var sut = new GenerateCouponCodeService(mockedLogger.Object);
            var result = sut.GenerateCouponCodes(request3Coupons7Length).Result;

            Assert.True(result.Count.Equals(3));
            Assert.All(result, x => x.Length.Equals(7));
        }

        [Fact]
        public void Generate_3_Coupons_Length_8_ExpectTrue()
        {
            var mockedLogger = _fixture.LoggerMock;

            var request3Coupons7Length = new GenerateCouponCodeRequest
            {
                CouponInstances = 3,
                CouponLength = 8,
            };

            var sut = new GenerateCouponCodeService(mockedLogger.Object);
            var result = sut.GenerateCouponCodes(request3Coupons7Length).Result;

            Assert.True(result.Count.Equals(3));
            Assert.All(result, x => x.Length.Equals(8));
        }

        [Fact]
        public void Generate_2000_Coupons_NotDuplicate_Length_7_ExpectTrue()
        {
            var mockedLogger = _fixture.LoggerMock;

            var request3Coupons7Length = new GenerateCouponCodeRequest
            {
                CouponInstances = 2000,
                CouponLength = 7,
            };

            var sut = new GenerateCouponCodeService(mockedLogger.Object);
            var result = sut.GenerateCouponCodes(request3Coupons7Length).Result;

            Assert.True(result.Count.Equals(2000));
            Assert.All(result, x => x.Length.Equals(7));
            Assert.True(result.Distinct().Count().Equals(2000));
        }

        [Fact]
        public void Generate_2000_Coupons_NotDuplicate_Length_8_ExpectTrue()
        {
            var mockedLogger = _fixture.LoggerMock;

            var request3Coupons7Length = new GenerateCouponCodeRequest
            {
                CouponInstances = 2000,
                CouponLength = 8,
            };

            var sut = new GenerateCouponCodeService(mockedLogger.Object);
            var result = sut.GenerateCouponCodes(request3Coupons7Length).Result;

            Assert.True(result.Count.Equals(2000));
            Assert.All(result, x => x.Length.Equals(8));
            Assert.True(result.Distinct().Count().Equals(2000));
        }

    }
}
