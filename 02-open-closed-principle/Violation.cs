using System;

namespace SOLID.OCP.Violation
{
    // ১. একটি পারফেক্ট ইন্টারফেস আছে, যার নিজস্ব মেথডও আছে।
    public interface IPaymentMethod
    {
        void ProcessPayment(decimal amount);
    }

    // ২. ক্লাসগুলো ইন্টারফেস ইমপ্লিমেন্ট করেছে।
    public class CreditCardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount}");
        }
    }

    public class PayPalPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount}");
        }
    }

    /// <summary>
    /// VIOLATION OF OCP:
    /// 
    /// বাস্তব প্রজেক্টে ডেভেলপাররা ঠিক এই ভুলটাই করে। 
    /// ইন্টারফেস আছে, পলিমরফিজমও কল হচ্ছে (নিচে payment.ProcessPayment)।
    /// 
    /// কিন্তু "Processing Fee" বের করার জন্য তারা ইন্টারফেসের সাহায্য না নিয়ে, 
    /// 'is' কিওয়ার্ড দিয়ে টাইপ চেক করে হার্ডকোড লজিক লিখে দেয়।
    /// 
    /// ভবিষ্যতে BkashPayment যোগ করতে হলে, বাধ্য হয়ে এই PaymentProcessor 
    /// ক্লাসের if-else এডিট করে নতুন ফি (Fee) লজিক যোগ করতে হবে (Modification)।
    /// </summary>
    public class PaymentProcessor
    {
        public void Process(IPaymentMethod payment, decimal amount)
        {
            decimal fee = 0;

            // ❌ OCP Violation: নতুন পেমেন্ট মেথড আসলে এখানে কোড এডিট করতে হবে!
            if (payment is CreditCardPayment)
            {
                fee = amount * 0.02m; // ২% ফি
            }
            else if (payment is PayPalPayment)
            {
                fee = amount * 0.05m; // ৫% ফি
            }

            decimal totalAmount = amount + fee;
            
            // পলিমরফিজম ঠিকই কাজ করছে, কিন্তু উপরের if/else ব্লকটি OCP লঙ্ঘন করেছে।
            payment.ProcessPayment(totalAmount);
        }
    }
}
