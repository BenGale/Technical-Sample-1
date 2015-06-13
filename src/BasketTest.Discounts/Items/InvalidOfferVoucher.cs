namespace BasketTest.Discounts.Items
{
    public class InvalidOfferVoucher
    {
        public readonly OfferVoucher Voucher;
        public readonly string Reason;

        public InvalidOfferVoucher(OfferVoucher voucher, string reason)
        {
            Voucher = voucher;
            Reason = reason;
        }
    }
}
