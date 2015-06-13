using System.Collections.Generic;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation.Offer
{
    public interface IOfferVoucherValidator
    {
        List<InvalidVoucher> Validate(
            List<Product> products, List<OfferVoucher> vouchers);
    }
}