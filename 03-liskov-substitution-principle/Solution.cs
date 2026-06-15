using System;

namespace SOLID.LSP.Solution
{
    // ১. সাধারণ পেমেন্ট ইন্টারফেস (যেটা সবাই করতে পারে)
    public interface IPaymentMethod
    {
        void ProcessPayment(decimal amount);
    }

    // ২. রিফান্ডেবল পেমেন্ট ইন্টারফেস (যেটা শুধু রিফান্ড করা যায় এমন মেথডগুলো করবে)
    public interface IRefundable
    {
        void Refund(decimal amount);
    }

    // ৩. ক্রেডিট কার্ড দুটোই করতে পারে
    public class CreditCardPayment : IPaymentMethod, IRefundable
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

    // ৪. গিফট কার্ড শুধু পেমেন্ট করতে পারে, রিফান্ড করতে পারে না!
    // তাই এটি শুধু IPaymentMethod ইমপ্লিমেন্ট করবে। IRefundable করবে না।
    public class GiftCardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Gift Card Processed: {amount}");
        }
    }

    /// <summary>
    /// SOLUTION OF LSP:
    /// RefundProcessor এখন আর IPaymentMethod এর উপর নির্ভর করে না।
    /// এটি শুধুমাত্র IRefundable এর উপর নির্ভর করে।
    /// তাই ভুল করে GiftCard পাস করার কোনো সুযোগ নেই, আর ক্র্যাশও করবে না।
    /// </summary>
    public class RefundProcessor
    {
        // এখানে শুধু রিফান্ডেবল পেমেন্টই দেওয়া যাবে
        public void ProcessRefunds(IRefundable[] refundablePayments, decimal amount)
        {
            foreach (var payment in refundablePayments)
            {
                // এখন আর ক্র্যাশ করার কোনো সম্ভাবনা নেই!
                payment.Refund(amount);
            }
        }
    }
}
