using System;

namespace BasketTest.Discounts.Items
{
    public class GiftVoucher
    {
        public readonly decimal Value;

        public GiftVoucher(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Value cannot be negative.");
            }

            Value = value;
        }
    }
}
