using System;

namespace BasketTest.Discounts.Items
{
    public sealed class OfferVoucher : Voucher
    {
        public readonly decimal Threshold;

        public OfferVoucher(decimal value, decimal threshold) : base(value)
        {
            if (threshold < 0)
            {
                throw new ArgumentException("Value cannot be negative.");
            }
            
            Threshold = threshold;
        }
    }
}
