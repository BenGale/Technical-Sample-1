using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation
{
    /// <summary>
    /// This validator will check that the value of gift vouchers
    /// does not exceed the value of the basket. We will attempt 
    /// to use most valuable vouchers first.
    /// </summary>
    public class GiftVoucherValueValidator : IVoucherValidator
    {
        public List<InvalidVoucher> Validate(
            List<Product> products, List<GiftVoucher> vouchers)
        {
            var runningTotal = products.Sum(p => p.Value);
            var invalidVouchers = new List<InvalidVoucher>();

            foreach (var voucher in vouchers.OrderByDescending(v => v.Value))
            {
                if (voucher.Value > runningTotal)
                {
                    invalidVouchers.Add(new InvalidVoucher(voucher,
                    "You have not reached the spend threshold for this voucher."));
                    continue;
                }
                runningTotal -= voucher.Value;
            }

            return invalidVouchers;
        } 
    }
}