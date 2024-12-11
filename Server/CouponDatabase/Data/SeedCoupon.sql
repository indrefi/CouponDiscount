MERGE INTO  [Coupon].[DiscountCoupon] AS TARGET
USING 
(
    VALUES 
    ('test-123', GETDATE(), GETDATE(), 0 ),
    ('test-456', GETDATE(), GETDATE(), 0 ),
    ('test-789', GETDATE(), GETDATE(), 0 )
) 
AS SOURCE ([CouponCode], [CreatedAt], [UpdatedAt], [IsUsed])
ON TARGET.CouponCode = SOURCE.CouponCode

WHEN NOT MATCHED 
THEN
    INSERT ([CouponCode], [CreatedAt], [UpdatedAt], [IsUsed]) VALUES (SOURCE.[CouponCode], SOURCE.[CreatedAt], SOURCE.[UpdatedAt], SOURCE.[IsUsed]);