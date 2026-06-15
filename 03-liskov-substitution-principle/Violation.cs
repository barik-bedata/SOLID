using System;

namespace SOLID.LSP.Violation2
{
    // ১. বেস ইন্টারফেস
    public interface IPaymentMethod
    {
        void ProcessPayment(decimal amount);
        void Refund(decimal amount);
    }

    // ২. ক্রেডিট কার্ড (সব মেথড সাপোর্ট করে)
    public class CreditCardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount) => Console.WriteLine("Card Processed");
        public void Refund(decimal amount) => Console.WriteLine("Card Refunded");
    }

    // ৩. গিফট কার্ড (রিফান্ড সাপোর্ট করে না)
    public class GiftCardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount) => Console.WriteLine("Gift Card Processed");
        
        // ❌ LSP Violation: ইন্টারফেসের মেথডকে এখানে জোর করে Exception থ্রো করা হচ্ছে।
        public void Refund(decimal amount)
        {
            throw new NotSupportedException("Gift Card cannot be refunded!");
        }
    }

    // ৪. রিফান্ড সার্ভিস
    public class RefundService
    {
        // ❌ এই মেথডটি যেকোনো IPaymentMethod গ্রহণ করে।
        // যদি GiftCardPayment পাস করা হয়, তাহলে এটি অ্যাপ্লিকেশন ক্র্যাশ করাবে।
        public void ExecuteRefund(IPaymentMethod payment, decimal amount)
        {
            payment.Refund(amount);
        }
    }

    /// <summary>
    /// Usage / Example:
    /// লুপ ব্যবহার না করে সরাসরি মেথড কল করে দেখানো হলো।
    /// </summary>
    public class Program
    {
        public void Run()
        {
            RefundService refundService = new RefundService();

            IPaymentMethod card = new CreditCardPayment();
            refundService.ExecuteRefund(card, 100); // ✅ ঠিকঠাক কাজ করবে

            // ❌ LSP Violation: GiftCardPayment পাস করলেই অ্যাপ্লিকেশন ক্র্যাশ করবে!
            // কারণ GiftCardPayment তার ইন্টারফেসের (IPaymentMethod) জায়গা নিতে পারেনি।
            IPaymentMethod giftCard = new GiftCardPayment();
            refundService.ExecuteRefund(giftCard, 100); // 💥 Crash! NotSupportedException
        }
    }
}
