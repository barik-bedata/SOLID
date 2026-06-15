using System;

namespace SOLID.OCP.Solution
{
    // ১. ফি (Fee) হিসাব করার দায়িত্বটাও ইন্টারফেসকে দিয়ে দেওয়া হলো
    public interface IPaymentMethod
    {
        decimal CalculateFee(decimal amount);
        void ProcessPayment(decimal amount);
    }

    // ২. এখন প্রতিটি ক্লাস নিজের ফি নিজেই হিসাব করতে পারে
    public class CreditCardPayment : IPaymentMethod
    {
        public decimal CalculateFee(decimal amount) => amount * 0.02m; // ২% ফি

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount}");
        }
    }

    public class PayPalPayment : IPaymentMethod
    {
        public decimal CalculateFee(decimal amount) => amount * 0.05m; // ৫% ফি

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount}");
        }
    }

    // ৩. নতুন বিকাশ যোগ করলে শুধু ক্লাস বানাতে হবে, আর কিচ্ছু করতে হবে না!
    public class BkashPayment : IPaymentMethod
    {
        public decimal CalculateFee(decimal amount) => amount * 0.01m; // ১% ফি

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing Bkash payment of {amount}");
        }
    }

    /// <summary>
    /// SOLUTION OF OCP:
    /// 
    /// এখন PaymentProcessor ক্লাসে কোনো if/else নেই। 
    /// নতুন যত পেমেন্ট মেথডই আসুক না কেন, এই ক্লাসের একটি লাইনও পরিবর্তন করতে হবে না।
    /// (Open for Extension, Closed for Modification)
    /// </summary>
    public class PaymentProcessor
    {
        public void Process(IPaymentMethod payment, decimal amount)
        {
            // পলিমরফিজম ব্যবহার করে ফি হিসাব করা হচ্ছে
            decimal fee = payment.CalculateFee(amount);
            decimal totalAmount = amount + fee;

            payment.ProcessPayment(totalAmount);
        }
    }
}
