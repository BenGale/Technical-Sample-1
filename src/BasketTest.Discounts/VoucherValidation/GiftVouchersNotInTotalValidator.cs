using System;
using System.Collections.Generic;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation
{
    public class GiftVouchersNotInTotalValidator : IVoucherValidator
    {
        public List<InvalidVoucher> Validate(List<Product> products, List<GiftVoucher> vouchers)
        {
            throw new NotImplementedException();
        }
    }
}
