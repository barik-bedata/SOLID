using System;

namespace SOLID.LSP.Violation
{
    // ১. বেস ক্লাস (অথবা ইন্টারফেস) যেখানে রিফান্ড করার মেথড আছে
    public abstract class PaymentMethod
    {
        public abstract void ProcessPayment(decimal amount);
        public abstract void Refund(decimal amount);
    }

    // ২. ক্রেডিট কার্ড পেমেন্ট (এটি প্রসেস এবং রিফান্ড দুটোই করতে পারে)
    public class CreditCardPayment : PaymentMethod
    {
        public override void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Credit Card Processed: {amount}");
        }

        public override void Refund(decimal amount)
        {
            Console.WriteLine($"Credit Card Refunded: {amount}");
        }
    }

    // ৩. গিফট কার্ড পেমেন্ট (এটি প্রসেস হতে পারে, কিন্তু রিফান্ড করা সম্ভব না!)
    public class GiftCardPayment : PaymentMethod
    {
        public override void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Gift Card Processed: {amount}");
        }
        
        public override void Refund(decimal amount)
        {
            // ❌ LSP Violation: বেস ক্লাসের মেথডকে এখানে জোর করে Exception থ্রো করা হচ্ছে।
            // কারণ গিফট কার্ডের টাকা রিফান্ড করা যায় না।
            throw new NotSupportedException("Refund is not supported for Gift Cards.");
        }
    }

    /// <summary>
    /// VIOLATION OF LSP:
    /// RefundProcessor আশা করছে যে সব PaymentMethod-ই রিফান্ড করতে পারবে।
    /// কিন্তু GiftCardPayment পাস করলে সিস্টেম ক্র্যাশ করবে! 
    /// সাব-ক্লাস (GiftCardPayment) তার বেস-ক্লাসের (PaymentMethod) জায়গা ঠিকমতো নিতে পারছে না।
    /// </summary>
    public class RefundProcessor
    {
        public void ProcessRefunds(PaymentMethod[] payments, decimal amount)
        {
            foreach (var payment in payments)
            {
                // যখনই লুপের মধ্যে GiftCardPayment আসবে, তখনই অ্যাপ্লিকেশন ক্র্যাশ করবে!
                payment.Refund(amount);
            }
        }
    }
}
