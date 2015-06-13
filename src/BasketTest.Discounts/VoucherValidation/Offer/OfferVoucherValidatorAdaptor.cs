using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation.Offer
{
    public class OfferVoucherValidatorAdaptor : IVoucherValidator
    {
        private readonly IOfferVoucherValidator _offerVoucherValidator;

        public OfferVoucherValidatorAdaptor(IOfferVoucherValidator offerVoucherValidator)
        {
            _offerVoucherValidator = offerVoucherValidator;
        }

        public List<InvalidVoucher> Validate(List<Product> products, List<Voucher> vouchers)
        {
            var offerVouchers = vouchers.OfType<OfferVoucher>().ToList();

            return _offerVoucherValidator.Validate(products, offerVouchers);
        }
    }
}