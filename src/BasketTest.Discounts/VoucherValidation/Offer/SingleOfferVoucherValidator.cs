using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation.Offer
{
    public class SingleOfferVoucherValidator : IOfferVoucherValidator
    {
        public List<InvalidVoucher> Validate(List<Product> products, List<OfferVoucher> vouchers)
        {
            var validVoucher = vouchers.OrderByDescending(v => v.Value).Take(1);
            var invalidVouchers = vouchers.Except(validVoucher)
                .Select(voucher => new InvalidVoucher(
                    voucher, "You may only have one offer voucher in the basket.")).ToList();

            return invalidVouchers;
        }
    }
}
