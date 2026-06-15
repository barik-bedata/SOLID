using System;

namespace SOLID.OCP.Violation
{
    public enum PaymentType
    {
        CreditCard,
        PayPal
    }

    public class PaymentInfo
    {
        public PaymentType Type { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// VIOLATION OF OCP:
    /// If we want to add a new payment method (e.g. Bkash or Stripe) in the future:
    /// 1. We must add a new value to the PaymentType enum.
    /// 2. We must modify the ProcessPayment method in this class to add another 'case' or 'if' block.
    /// This violates OCP because the class is not closed for modification.
    /// </summary>
    public class PaymentProcessor
    {
        public void ProcessPayment(PaymentInfo payment)
        {
            if (payment.Type == PaymentType.CreditCard)
            {
                // Credit Card processing logic
                Console.WriteLine($"Processing credit card payment of {payment.Amount}");
            }
            else if (payment.Type == PaymentType.PayPal)
            {
                // PayPal processing logic
                Console.WriteLine($"Processing PayPal payment of {payment.Amount}");
            }
        }
    }
}
