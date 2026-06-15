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

    /// <summary>
    /// PART 2: Explanation of "Parent should be replaceable by child"
    /// </summary>
    public class LspExplanation
    {
        public void Explain()
        {
            // একদম সহজ কথায়:
            // IPaymentMethod (Parent) হলো একটি 'ফাঁকা বাক্স' বা 'লেবেল'।
            // CreditCardPayment (Child) বা GiftCardPayment (Child) হলো আসল কাজ করার 'মেশিন'।

            // লাইন ১: আমরা বলছি, "আমাকে IPaymentMethod নামের একটি বাক্স দাও, 
            // আর সেই বাক্সের ভেতরে আসল CreditCardPayment মেশিনটি ঢুকিয়ে দাও।"
            // অর্থাৎ, এখানে Parent (IPaymentMethod) এর জায়গাটি দখল করেছে Child (CreditCardPayment)।
            IPaymentMethod pay1 = new CreditCardPayment(); 
            
            // লাইন ২: একইভাবে, Parent এর জায়গা দখল করেছে আরেকটি Child (GiftCardPayment)।
            IPaymentMethod pay2 = new GiftCardPayment();

            // LSP এর মূল নিয়ম হলো: 
            // এই Parent (IPaymentMethod) এর বাক্সে আপনি যেই Child (মেশিন)-কেই রাখেন না কেন, 
            // বাক্সের বাটন (যেমন ProcessPayment) চাপলে তা ঠিকঠাক কাজ করতে হবে। কোনোভাবেই সিস্টেম ক্র্যাশ করা যাবে না।
            pay1.ProcessPayment(100); // Works perfectly
            pay2.ProcessPayment(100); // Works perfectly
        }
    }
}
