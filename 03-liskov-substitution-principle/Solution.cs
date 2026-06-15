using System;

namespace SOLID.LSP.Solution2
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
        public void ProcessPayment(decimal amount) => Console.WriteLine("Card Processed");
        public void Refund(decimal amount) => Console.WriteLine("Card Refunded");
    }

    // ৪. গিফট কার্ড শুধু পেমেন্ট করতে পারে, রিফান্ড করতে পারে না!
    public class GiftCardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount) => Console.WriteLine("Gift Card Processed");
    }

    // ৫. রিফান্ড সার্ভিস (এটি এখন শুধু IRefundable গ্রহণ করে)
    public class RefundService
    {
        // ✅ এখন এটি শুধুমাত্র IRefundable ইন্টারফেস গ্রহণ করে
        public void ExecuteRefund(IRefundable payment, decimal amount)
        {
            payment.Refund(amount);
        }
    }

    /// <summary>
    /// Usage / Example:
    /// </summary>
    public class Program
    {
        public void Run()
        {
            RefundService refundService = new RefundService();

            CreditCardPayment card = new CreditCardPayment();
            refundService.ExecuteRefund(card, 100); // ✅ ঠিকঠাক কাজ করবে

            GiftCardPayment giftCard = new GiftCardPayment();
            
            // 🚫 Compiler Error! Safe!
            // refundService.ExecuteRefund(giftCard, 100); 
            // ভুল করে GiftCardPayment পাস করার কোনো সুযোগই নেই, 
            // কারণ এটি IRefundable ইমপ্লিমেন্ট করে না। সিস্টেম পুরোপুরি ক্র্যাশ-ফ্রি!
        }
    }
}
