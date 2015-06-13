using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation.Gift
{
    public class GiftVoucherValidatorAdaptor : IVoucherValidator
    {
        private readonly IGiftVoucherValidator _giftVoucherValidator;

        public GiftVoucherValidatorAdaptor(IGiftVoucherValidator giftVoucherValidator)
        {
            _giftVoucherValidator = giftVoucherValidator;
        }

        public List<InvalidVoucher> Validate(
            List<Product> products, List<Voucher> vouchers)
        {
            var giftVouchers = vouchers.OfType<GiftVoucher>().ToList();

            return _giftVoucherValidator.Validate(products, giftVouchers);
        }
    }
}
