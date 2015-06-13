using System;
using BasketTest.Discounts.Enums;

namespace BasketTest.Discounts.Items
{
    public sealed class OfferVoucher : Voucher
    {
        public readonly decimal Threshold;
        public readonly ProductCategory? CategoryRestriction;

        public OfferVoucher(decimal value, decimal threshold, ProductCategory? categoryRestriction = null) : base(value)
        {
            if (threshold < 0)
            {
                throw new ArgumentException("Value cannot be negative.");
            }
            
            Threshold = threshold;
            CategoryRestriction = categoryRestriction;
        }
    }
}
