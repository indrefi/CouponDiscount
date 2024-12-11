IF NOT EXISTS 
(
    SELECT 0 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Coupon' 
    AND TABLE_NAME = 'DiscountCoupon'
)
BEGIN
    CREATE TABLE [Coupon].[DiscountCoupon] 
    (
	    [DiscountCouponID] INT NOT NULL IDENTITY,	
        [CouponCode] VARCHAR(8) NOT NULL,
	    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME NULL DEFAULT GETDATE(), 
        [IsUsed] BIT NOT NULL DEFAULT(0),

        CONSTRAINT [PK_Coupon_DiscountCoupon] PRIMARY KEY (DiscountCouponID),
        CONSTRAINT [UQ_Coupon_DiscontCoupon_CouponCode] UNIQUE (CouponCode)

    )
END 

IF NOT EXISTS(SELECT 0 FROM sys.indexes WHERE name='IX_NC_Coupon_DiscountCoupon_CuponCode' AND object_id = OBJECT_ID('Coupon.DiscountCoupon'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_NC_Coupon_DiscountCoupon_CuponCode]
    ON [Coupon].[DiscountCoupon](CouponCode ASC);
END

IF NOT EXISTS(SELECT 0 FROM sys.indexes WHERE name='IX_NC_Coupon_DiscountCoupon_IsUsed' AND object_id = OBJECT_ID('Coupon.DiscountCoupon'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_NC_Coupon_DiscountCoupon_IsUsed]
    ON [Coupon].[DiscountCoupon](IsUsed ASC);
END


