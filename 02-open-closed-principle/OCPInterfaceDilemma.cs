using System;

namespace SOLID.OCP.InterfaceDilemma
{
    // =================================================================
    // ❌ THE PROBLEM SCENARIO: DIRECT MODIFICATION
    // =================================================================
    
    public interface IOriginalPaymentMethod
    {
        void Process(decimal amount);
        
        // If we add this here directly:
        // void VerifyOtp(); 
        // 
        // It will immediately break PayPalPayment below because PayPalPayment does NOT 
        // implement VerifyOtp(). We would be forced to modify PayPalPayment.
    }

    public class PayPalPaymentOriginal : IOriginalPaymentMethod
    {
        public void Process(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount}");
        }
        
        // FORCED TO ADD THIS:
        // public void VerifyOtp() { throw new NotImplementedException(); }
    }


    // =================================================================
    // 🟢 SOLUTION 1: INTERFACE INHERITENCE (ইন্টারফেস ইনহেরিটেন্স)
    // =================================================================
    
    // Step 1: Keep the base interface untouched
    public interface IPaymentMethod
    {
        void Process(decimal amount);
    }

    // Step 2: Create a NEW interface for the new behavior that inherits the base
    public interface ISecurePaymentMethod : IPaymentMethod
    {
        void VerifyOtp(); // Only secure methods will have this
    }

    // PayPal only needs the base interface. We don't touch PayPal at all!
    public class PayPalPayment : IPaymentMethod
    {
        public void Process(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount}");
        }
    }

    // Credit Card needs BOTH process and OTP verification
    public class CreditCardPayment : ISecurePaymentMethod
    {
        public void Process(decimal amount)
        {
            Console.WriteLine($"Processing Credit Card payment of {amount}");
        }

        public void VerifyOtp()
        {
            Console.WriteLine("OTP Verification successful for Credit Card.");
        }
    }


    // =================================================================
    // 🟢 SOLUTION 2: EXTENSION METHOD (এক্সটেনশন মেথড)
    // =================================================================
    // We want to add a "PrintReceipt" feature to all payment methods,
    // without changing the IPaymentMethod interface or any of the classes.

    public static class PaymentExtensions
    {
        // Notice the 'this' keyword before IPaymentMethod.
        // This makes PrintReceipt behave like a method of IPaymentMethod.
        public static void PrintReceipt(this IPaymentMethod paymentMethod, decimal amount)
        {
            Console.WriteLine($"--- RECEIPT ---");
            Console.WriteLine($"Payment Method Type: {paymentMethod.GetType().Name}");
            Console.WriteLine($"Amount Processed: {amount:C}");
            Console.WriteLine($"----------------");
        }
    }


    // =================================================================
    // 🟢 SOLUTION 3: DECORATOR PATTERN (ডেকোরেটর প্যাটার্ন)
    // =================================================================
    // We want to add SMS Notification behavior after a payment processes,
    // without modifying the PayPalPayment or CreditCardPayment class.

    public class PaymentWithSmsDecorator : IPaymentMethod
    {
        private readonly IPaymentMethod _innerPaymentMethod;

        // We wrap/decorate any existing payment method
        public PaymentWithSmsDecorator(IPaymentMethod innerPaymentMethod)
        {
            _innerPaymentMethod = innerPaymentMethod;
        }

        public void Process(decimal amount)
        {
            // 1. Run the original payment method
            _innerPaymentMethod.Process(amount);

            // 2. Add the new SMS behavior
            SendSmsNotification(amount);
        }

        private void SendSmsNotification(decimal amount)
        {
            Console.WriteLine($"[SMS NOTIFICATION] A payment of {amount:C} was successfully processed!");
        }
    }


    // =================================================================
    // 🖥️ EXECUTION DEMO
    // =================================================================
    public class Program
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== OCP SOLUTIONS DEMO ===");

            // --- Solution 1 Demo (Interface Inheritance) ---
            IPaymentMethod paypal = new PayPalPayment();
            paypal.Process(100); // Only processes payment, no OTP needed

            ISecurePaymentMethod creditCard = new CreditCardPayment();
            creditCard.Process(250);
            creditCard.VerifyOtp(); // Verifies OTP securely

            // --- Solution 2 Demo (Extension Method) ---
            // Even though IPaymentMethod doesn't have PrintReceipt inside it, 
            // we can call it directly thanks to Extension Methods!
            paypal.PrintReceipt(100);
            creditCard.PrintReceipt(250);

            // --- Solution 3 Demo (Decorator Pattern) ---
            // We wrap PayPalPayment with our SMS Decorator to add SMS notifications.
            // Notice how PayPalPayment code itself remains 100% unchanged!
            IPaymentMethod decoratedPaypal = new PaymentWithSmsDecorator(paypal);
            decoratedPaypal.Process(100); 
        }
    }
}
