using System.Collections.Generic;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation
{
    public interface IGiftVoucherValidator
    {
        List<InvalidVoucher> Validate(
            List<Product> products, List<GiftVoucher> vouchers);
    }
}