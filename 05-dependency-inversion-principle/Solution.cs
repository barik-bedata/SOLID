using System;

namespace SOLID.DIP.Solution
{
    // ✅ Abstraction (ইন্টারফেস) - হাই-লেভেল এবং লো-লেভেল উভয়েই এখন এর উপর নির্ভর করবে।
    public interface INotificationService
    {
        void SendMessage(string message);
    }

    // Low-Level Module 1 (Email)
    public class EmailSender : INotificationService
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"Sending Email: {message}");
        }
    }

    // Low-Level Module 2 (SMS) - নতুন যোগ করা হলো
    public class SmsSender : INotificationService
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"Sending SMS: {message}");
        }
    }

    // High-Level Module
    public class OrderProcessor
    {
        private readonly INotificationService _notificationService;

        // ✅ Constructor Injection: হাই-লেভেল মডিউল এখন আর কোনো নির্দিষ্ট লো-লেভেল ক্লাসের উপর নির্ভর করে না।
        // সে শুধু Abstraction (INotificationService) এর উপর নির্ভর করে। একেই বলে Loose Coupling!
        public OrderProcessor(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ProcessOrder()
        {
            Console.WriteLine("Order Processed Successfully.");
            _notificationService.SendMessage("Your order has been confirmed.");
        }
    }
    
    /// <summary>
    /// Usage / Example:
    /// </summary>
    public class Program
    {
        public void Run()
        {
            // Email দিয়ে পাঠাতে চাইলে
            OrderProcessor emailProcessor = new OrderProcessor(new EmailSender());
            emailProcessor.ProcessOrder();

            // SMS দিয়ে পাঠাতে চাইলে (OrderProcessor-এ কোনো চেঞ্জই করা লাগল না!)
            OrderProcessor smsProcessor = new OrderProcessor(new SmsSender());
            smsProcessor.ProcessOrder();
        }
    }
}
