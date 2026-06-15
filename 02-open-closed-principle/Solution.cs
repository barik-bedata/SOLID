using System;

namespace SOLID.OCP.Solution
{
    // --- 1. Define the abstraction (Open for Extension) ---
    public interface IPaymentMethod
    {
        void Process(decimal amount);
    }

    // --- 2. Concrete implementations ---
    public class CreditCardPayment : IPaymentMethod
    {
        public void Process(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount}");
        }
    }

    public class PayPalPayment : IPaymentMethod
    {
        public void Process(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount}");
        }
    }

    // --- 3. Adding a new feature (Bkash) without modifying existing code ---
    public class BkashPayment : IPaymentMethod
    {
        public void Process(decimal amount)
        {
            Console.WriteLine($"Processing Bkash payment of {amount}");
        }
    }

    // --- 4. The Orchestrator (Closed for Modification) ---
    /// <summary>
    /// SOLUTION OF OCP:
    /// This class is open for extension (we can pass any new IPaymentMethod implementation)
    /// but closed for modification (we don't need to change this class to support new payment methods).
    /// </summary>
    public class PaymentProcessor
    {
        public void ProcessPayment(IPaymentMethod paymentMethod, decimal amount)
        {
            paymentMethod.Process(amount);
        }
    }
}
