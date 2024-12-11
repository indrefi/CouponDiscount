using Grpc.Net.Client;
using System;
using CouponService;
using Grpc.Core;

namespace CouponGenerationClient
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // Create a gRPC channel for communication with the server
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            // Create a client for the Greeter service
            var generateClient = new GenerateDiscountCouponService.GenerateDiscountCouponServiceClient(channel);
            var useClient = new UseDiscountCouponService.UseDiscountCouponServiceClient(channel);
            var getCouponsClient = new GetUnusedCouponService.GetUnusedCouponServiceClient(channel);
            var getTokenClient = new GetTokenService.GetTokenServiceClient(channel);

            var jwtToken = string.Empty;

            bool isMarkedForExit = false;
            while (!isMarkedForExit)
            {
                Console.WriteLine("Do you want to send a request Y\\N ? ");
                var isSendRequestTrue = Console.ReadLine();

                if (isSendRequestTrue.ToLower().Equals("y")) 
                {
                    if (jwtToken.Equals(string.Empty))
                    {
                        // Dummy assign will expire after 30 min and has to renew the token by calling the GetTokenOption in the menu.
                        jwtToken = OptionGetToken(getTokenClient);
                    }

                    Console.Clear();
                    Console.WriteLine("1. Generate Code");
                    Console.WriteLine("2. Use Code");
                    Console.WriteLine("3. Get Unused Coupon Codes");
                    Console.WriteLine("4. Get Token");
                    Console.WriteLine("5. Exit");
                    var optionSelected = Console.ReadLine();

                    switch (optionSelected)
                    {
                        case "1":
                            OptionGenerate(generateClient, jwtToken);

                            break;
                        case "2":
                            OptionUse(useClient, jwtToken);

                            break;
                        case "3":
                            OptionGetCoupons(getCouponsClient, jwtToken);

                            break;

                        case "4":
                            OptionGetToken(getTokenClient);

                            break;
                        default:
                            isMarkedForExit = true;

                            break;
                    }
                    
                }
                else
                {
                    isMarkedForExit = true;

                    break;
                }
               
            }
      
        }

        #region Option Selected

        private static void OptionGenerate(GenerateDiscountCouponService.GenerateDiscountCouponServiceClient client, string jwtToken)
        {
            var metadata = new Metadata
            {
                { "Authorization", $"Bearer {jwtToken}" }
            };

            // Prepare the request
            var couponVales = ValidateGenerateInput();
            var request = new GenerateRequest { Count = (uint)couponVales.Item1, Length = couponVales.Item2 };

            // Call the RPC method
            var response = client.Generate(request, metadata).Result;

            // Print the response from the server
            Console.WriteLine("Response: " + response);

        }

        private static string OptionGetToken(GetTokenService.GetTokenServiceClient client)
        {
            var jwtToken = string.Empty;

            // Prepare the request
            var authValues = ValidateAuthInput();
            var request = new GetTokenRequest { UserName = authValues.Item1, UserPassword = authValues.Item2 };

            // Call the RPC method
            var response = client.GetToken(request);

            // Print the response from the server
            Console.WriteLine("Response: " + response);

            // Update the existing token value;
            if(response != null && response.Token != null) { jwtToken = response.Token.ToString(); }

            return jwtToken;

        }

        private static void OptionGetCoupons(GetUnusedCouponService.GetUnusedCouponServiceClient client, string jwtToken)
        {
            var metadata = new Metadata
            {
                { "Authorization", $"Bearer {jwtToken}" }
            };

            // Prepare the request
            var couponFetchNumber = ValidateGetUnusedCouponsInput();
            var request = new GetUnusedCouponsRequest { CouponsNumber = (uint)couponFetchNumber };

            // Call the RPC method
            var response = client.GetCoupons(request, metadata).Coupons;

            foreach(var coupon in response)
            {
                // Print the response from the server
                Console.WriteLine("CouponCode: " + coupon);
            }         
        }

        private static void OptionUse(UseDiscountCouponService.UseDiscountCouponServiceClient client, string jwtToken)
        {
            var metadata = new Metadata
            {
                { "Authorization", $"Bearer {jwtToken}" }
            };

            // Prepare the request
            var couponValue = ValidateUseCodeInput();
            var request = new UseCodeRequest { Code = couponValue };

            // Call the RPC method
            var response = client.UseCoupon(request, metadata).Result;

            // Print the response from the server
            Console.WriteLine("Response: " + response);
        }

        #endregion Options Selected


        #region Input Validator

        private static (string, string) ValidateAuthInput()
        {
            Console.WriteLine("Enter UserName");
            var user = Console.ReadLine();

            while (true)
            {
                if (!(user != string.Empty || user != null))
                {
                    Console.WriteLine("Invalid value. Enter again.");
                    user = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Enter Password");
            var password = Console.ReadLine();
            while (true)
            {
                if (!(password != string.Empty || user != null))
                {
                    Console.WriteLine("Invalid value. Enter again.");
                    password = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            return (user, password);
        }

        private static (int,int) ValidateGenerateInput()
        {
            Console.WriteLine("How many discount coupons do you want to generate?");
            var couponNumber = Console.ReadLine();
            int couponNumberValue;
            var parseResult = int.TryParse(couponNumber, out couponNumberValue);

            while (true)
            {              
                if (!(parseResult && couponNumberValue > 0 && couponNumberValue <= 2000))
                {
                    Console.WriteLine("Invalid value. Enter again.");
                    couponNumber = Console.ReadLine();
                    parseResult = int.TryParse(couponNumber, out couponNumberValue);
                }
                else
                {
                    break;
                }
            }
            
            Console.WriteLine("What should be the length of the coupon 7 or 8 ?");
            var couponLength = Console.ReadLine();
            int counponLengthValue;
            parseResult = int.TryParse(couponLength, out counponLengthValue);

            while (true)
            {
                if (!(parseResult && (counponLengthValue == 7 || counponLengthValue == 8)))
                {
                    Console.WriteLine("Invalid value. Enter again.");
                    couponLength = Console.ReadLine();
                    parseResult = int.TryParse(couponLength, out counponLengthValue);
                }
                else
                {
                    break;
                }
            }

            return (couponNumberValue, counponLengthValue);
        }

        private static int ValidateGetUnusedCouponsInput()
        {
            Console.WriteLine("How many discount coupons do you want to fetch?");
            var couponNumber = Console.ReadLine();
            int couponNumberValue;
            var parseResult = int.TryParse(couponNumber, out couponNumberValue);

            while (true)
            {
                if (!(parseResult && couponNumberValue > 0 && couponNumberValue <= 2000))
                {
                    Console.WriteLine("Invalid value. Enter again.");
                    couponNumber = Console.ReadLine();
                    parseResult = int.TryParse(couponNumber, out couponNumberValue);
                }
                else
                {
                    break;
                }
            }

            return couponNumberValue;
        }

        private static string ValidateUseCodeInput()
        {
            var result = string.Empty;

            Console.WriteLine("Enter the discount coupon code");
            var couponValue = Console.ReadLine();

            while (true)
            {
                if (!(couponValue.Length >=7 && couponValue.Length <=8))
                {
                    Console.WriteLine("Invalid value. Enter again.");
                    couponValue = Console.ReadLine();
                }
                else
                {
                    result = couponValue;

                    break;
                }
            }

            return result;
        }

        #endregion Input Validator
    }
}
