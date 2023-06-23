namespace Domain.Entities
{
    public enum SaleStatus
    {
        AwaitingPayment,
        PaymentApproved,
        SentToCarrier,
        Delivered,
        Canceled
    }
}
