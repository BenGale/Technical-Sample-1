using System.Collections.Generic;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation
{
    public interface IVoucherValidator
    {
        List<InvalidVoucher> Validate(
            List<Product> products, List<Voucher> vouchers);
    }
}