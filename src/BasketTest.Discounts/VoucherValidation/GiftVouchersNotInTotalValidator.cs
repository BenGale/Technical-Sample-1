using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Enums;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation
{
    public class GiftVouchersNotInTotalValidator : IVoucherValidator
    {
        private readonly GiftVoucherValueValidator _valueValidator;

        public GiftVouchersNotInTotalValidator(
            GiftVoucherValueValidator valueValidator)
        {
            _valueValidator = valueValidator;
        }

        public List<InvalidVoucher> Validate(
            List<Product> products, List<GiftVoucher> vouchers)
        {
            var productsWithoutGiftVouchers = products
                .Where(p => p.Category != ProductCategory.GiftVoucher).ToList();

            return _valueValidator.Validate(productsWithoutGiftVouchers, vouchers);
        }
    }
}