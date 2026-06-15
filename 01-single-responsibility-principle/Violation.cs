using System;
using System.IO;
using System.Net.Mail;
using System.Text.Json;

namespace SOLID.SRP.Violation
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
    }

    /// <summary>
    /// VIOLATION OF SRP:
    /// This class has multiple reasons to change:
    /// 1. Business rules for calculating total price/tax change.
    /// 2. Database schema, storage format (JSON file in this case), or persistence mechanism changes.
    /// 3. Email sending configuration, provider, or message template changes.
    /// </summary>
    public class OrderProcessor
    {
        public void Process(Order order)
        {
            // 1. Calculate Total Amount (Tax calculation business logic)
            decimal total = order.Amount + (order.Amount * order.TaxRate);
            Console.WriteLine($"Calculated total: {total}");

            // 2. Persist the Order to Database/Storage (Persistence logic)
            var orderJson = JsonSerializer.Serialize(order);
            File.WriteAllText($"order_{order.Id}.json", orderJson);
            Console.WriteLine($"Saved order {order.Id} to file database.");

            // 3. Send Confirmation Email (Notification logic)
            using (var message = new MailMessage("no-reply@mycompany.com", order.CustomerEmail))
            {
                message.Subject = "Order Processed Successfully";
                message.Body = $"Hi! Your order of {total:C} has been successfully processed.";
                
                using (var client = new SmtpClient("smtp.mycompany.com"))
                {
                    // Simulated sending
                    Console.WriteLine($"Sending SMTP email to {order.CustomerEmail} via smtp.mycompany.com...");
                    // client.Send(message); 
                }
            }
        }
    }
}
