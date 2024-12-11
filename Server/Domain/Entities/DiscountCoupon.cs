using System;

namespace Domain.Entities
{
    public class DiscountCoupon
    {
        public int DiscountCouponID { get; set; }
        public string CouponCode { get; set; }
        public DateTime CreatedAt { get;set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
