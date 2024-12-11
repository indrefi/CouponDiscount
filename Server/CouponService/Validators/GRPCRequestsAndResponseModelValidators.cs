using System.Text.RegularExpressions;

namespace CouponService.Validators
{
    public static class GRPCRequestsAndResponseModelValidators
    {
        public static bool Validate(this GenerateRequest request) 
        {
            return (request.Length.Equals(7) || request.Length.Equals(8)) && request.Count > 0 && request.Count <= 2000;
        }

        public static bool Validate(this UseCodeRequest request)
        {
            return request.Code.Length >= 7 && request.Code.Length <= 8;
        }

        public static bool Validate(this GetUnusedCouponsRequest request)
        {
            return request.CouponsNumber > 0 && request.CouponsNumber <= 2000;
        }

        public static bool Validate(this GetTokenRequest request)
        {
            string pattern = "^[a-zA-Z0-9]*$";
            return Regex.IsMatch(request.UserName, pattern);
        }
    }
}