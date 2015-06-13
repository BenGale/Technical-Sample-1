using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Enums;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation.Offer
{
    public class OfferVoucherThresholdValidator : IOfferVoucherValidator
    {
        private readonly IOfferVoucherValidator _singleOfferVoucherValidator;

        public OfferVoucherThresholdValidator(IOfferVoucherValidator singleOfferVoucherValidator)
        {
            _singleOfferVoucherValidator = singleOfferVoucherValidator;
        }

        public List<InvalidVoucher> Validate(List<Product> products, List<OfferVoucher> vouchers)
        {
            var productsWithoutGiftVouchers = products
                .Where(p => p.Category != ProductCategory.GiftVoucher).ToList();
            var basketTotal = productsWithoutGiftVouchers.Sum(v => v.Value);
            var invalidVouchers = new List<InvalidVoucher>();
            var validVouchers = new List<OfferVoucher>();

            foreach (var offerVoucher in vouchers)
            {
                if (offerVoucher.Threshold > basketTotal)
                {
                    invalidVouchers.Add(new InvalidVoucher(offerVoucher,
                        "You have not reached the spend threshold. " +
                        $"Spend another {((offerVoucher.Threshold - basketTotal) + 0.01m).ToString("C")} " +
                        $"to receive {offerVoucher.Value.ToString("C")} discount."));
                    continue;
                }
                validVouchers.Add(offerVoucher);
            }

            invalidVouchers.AddRange(_singleOfferVoucherValidator.Validate(products, validVouchers));
            return invalidVouchers;
        }
    }
}
