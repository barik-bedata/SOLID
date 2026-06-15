using System;

namespace SOLID.DIP.Violation
{
    // Low-Level Module (Specific Detail)
    public class EmailSender
    {
        public void SendEmail(string message)
        {
            Console.WriteLine($"Sending Email: {message}");
        }
    }

    // High-Level Module (Core Business Logic)
    public class OrderProcessor
    {
        // ❌ DIP Violation: হাই-লেভেল মডিউল সরাসরি লো-লেভেল মডিউলের (EmailSender) উপর নির্ভর করছে।
        // যদি ভবিষ্যতে ইমেইলের বদলে SMS পাঠাতে চাই, তবে পুরো ক্লাস চেঞ্জ করতে হবে!
        private readonly EmailSender _emailSender;

        public OrderProcessor()
        {
            // সরাসরি Concrete Class এর ইনস্ট্যান্স তৈরি করা হচ্ছে (Tight Coupling)
            _emailSender = new EmailSender();
        }

        public void ProcessOrder()
        {
            Console.WriteLine("Order Processed Successfully.");
            
            // নোটিফিকেশন পাঠানোর জন্য সরাসরি EmailSender ব্যবহার করা হচ্ছে
            _emailSender.SendEmail("Your order has been confirmed.");
        }
    }
}
