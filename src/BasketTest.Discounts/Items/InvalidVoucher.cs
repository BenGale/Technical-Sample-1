namespace BasketTest.Discounts.Items
{
    public class InvalidVoucher
    {
        public readonly Voucher Voucher;
        public readonly string Reason;

        public InvalidVoucher(Voucher voucher, string reason)
        {
            Voucher = voucher;
            Reason = reason;
        }
    }
}
