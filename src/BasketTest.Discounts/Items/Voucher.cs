using System;

namespace BasketTest.Discounts.Items
{
    public abstract class Voucher
    {
        public readonly decimal Value;

        protected Voucher(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Value cannot be negative.");
            }

            Value = value;
        }
    }
}