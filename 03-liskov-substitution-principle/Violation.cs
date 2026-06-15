using System;

namespace SOLID.LSP.Violation
{
    // ১. বেস ইন্টারফেস যেখানে পেমেন্ট এবং রিফান্ড করার মেথড আছে
    public interface IPaymentMethod
    {
        void ProcessPayment(decimal amount);
        void Refund(decimal amount);
    }

    // ২. ক্রেডিট কার্ড পেমেন্ট (এটি প্রসেস এবং রিফান্ড দুটোই করতে পারে)
    public class CreditCardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Credit Card Processed: {amount}");
        }

        public void Refund(decimal amount)
        {
            Console.WriteLine($"Credit Card Refunded: {amount}");
        }
    }

    // ৩. গিফট কার্ড পেমেন্ট (এটি প্রসেস হতে পারে, কিন্তু রিফান্ড করা সম্ভব না!)
    public class GiftCardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Gift Card Processed: {amount}");
        }
        
        public void Refund(decimal amount)
        {
            // ❌ LSP Violation: ইন্টারফেসের মেথডকে এখানে জোর করে Exception থ্রো করা হচ্ছে।
            // কারণ গিফট কার্ডের টাকা রিফান্ড করা যায় না।
            throw new NotSupportedException("Refund is not supported for Gift Cards.");
        }
    }

    /// <summary>
    /// VIOLATION OF LSP:
    /// RefundProcessor আশা করছে যে সব IPaymentMethod-ই রিফান্ড করতে পারবে।
    /// কিন্তু GiftCardPayment পাস করলে সিস্টেম ক্র্যাশ করবে! 
    /// ইমপ্লিমেন্টিং ক্লাস (GiftCardPayment) তার ইন্টারফেসের (IPaymentMethod) জায়গা ঠিকমতো নিতে পারছে না।
    /// </summary>
    public class RefundProcessor
    {
        public void ProcessRefunds(IPaymentMethod[] payments, decimal amount)
        {
            foreach (var payment in payments)
            {
                // যখনই লুপের মধ্যে GiftCardPayment আসবে, তখনই অ্যাপ্লিকেশন ক্র্যাশ করবে!
                payment.Refund(amount);
            }
        }
    }
}
