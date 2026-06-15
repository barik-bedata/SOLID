using System;

namespace SOLID.OCP.Violation
{
    // Concrete payment classes without a shared polymorphic behavior
    public class CreditCardPayment
    {
        public void PayWithCard(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount}");
        }
    }

    public class PayPalPayment
    {
        public void PayWithPayPal(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount}");
        }
    }

    /// <summary>
    /// VIOLATION OF OCP:
    /// This processor must check the concrete class type to call the appropriate method.
    /// If we want to add a new payment method (e.g. BkashPayment) in the future:
    /// We must modify this ProcessPayment method to add another 'if/else' check for BkashPayment.
    /// This violates OCP because the processor is not closed for modification.
    /// </summary>
    public class PaymentProcessor
    {
        public void ProcessPayment(object payment, decimal amount)
        {
            if (payment is CreditCardPayment cardPayment)
            {
                cardPayment.PayWithCard(amount);
            }
            else if (payment is PayPalPayment payPalPayment)
            {
                payPalPayment.PayWithPayPal(amount);
            }
            // If we add Bkash, we have to modify this file:
            // else if (payment is BkashPayment bkash) { ... }
        }
    }
}
