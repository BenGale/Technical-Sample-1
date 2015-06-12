namespace BasketTest.Discounts.Items
{
    public class InvalidVoucher
    {
        public readonly GiftVoucher Voucher;
        public readonly string Reason;

        public InvalidVoucher(GiftVoucher voucher, string reason)
        {
            Voucher = voucher;
            Reason = reason;
        }
    }
}
