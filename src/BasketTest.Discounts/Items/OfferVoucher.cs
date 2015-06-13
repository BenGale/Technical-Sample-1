using System;

namespace BasketTest.Discounts.Items
{
    public class OfferVoucher
    {
        public readonly decimal Value;
        public readonly decimal Threshold;

        public OfferVoucher(decimal value, decimal threshold)
        {
            if (value < 0 || threshold < 0)
            {
                throw new ArgumentException("Value cannot be negative.");
            }

            Value = value;
            Threshold = threshold;
        }
    }
}
