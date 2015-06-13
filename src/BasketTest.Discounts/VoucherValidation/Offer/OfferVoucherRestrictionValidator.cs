using System;
using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts.VoucherValidation.Offer
{
    public class OfferVoucherRestrictionValidator : IOfferVoucherValidator
    {
        private readonly IOfferVoucherValidator _offerThresholdValidator;

        public OfferVoucherRestrictionValidator(IOfferVoucherValidator offerThresholdValidator)
        {
            _offerThresholdValidator = offerThresholdValidator;
        }

        public List<InvalidVoucher> Validate(
            List<Product> products, List<OfferVoucher> vouchers)
        {
            var groupedProducts = products.GroupBy(product => product.Category)
                .Select(grouping => new
                {
                    category = grouping.Key,
                    totalValue = grouping.Sum(g => g.Value)
                }).ToList();
            var invalidVouchers = new List<InvalidVoucher>();
            var validVouchers = new List<OfferVoucher>();

            foreach (var offerVoucher in vouchers)
            {
                if (offerVoucher.CategoryRestriction == null)
                {
                    validVouchers.Add(offerVoucher);
                    continue;
                }

                var productsInCaegory = groupedProducts.SingleOrDefault(
                    p => p.category == offerVoucher.CategoryRestriction);
                if (productsInCaegory == null)
                {
                    invalidVouchers.Add(new InvalidVoucher(offerVoucher,
                        "There are no products in your basket applicable to this voucher."));
                    continue;
                }

                var productsValue = productsInCaegory.totalValue;
                if (productsValue < offerVoucher.Value)
                {
                    invalidVouchers.Add(new InvalidVoucher(offerVoucher,
                        "You have no spent enough in this product category."));
                    continue;
                }
                validVouchers.Add(offerVoucher);
            }

            invalidVouchers.AddRange(_offerThresholdValidator.Validate(products, validVouchers));
            return invalidVouchers;
        }
    }
}
