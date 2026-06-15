using System;
using System.IO;
using System.Net.Mail;
using System.Text.Json;

namespace SOLID.SRP.Solution
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
    }

    // --- 1. TAX CALCULATION (Business Logic) ---
    public interface ITaxCalculator
    {
        decimal CalculateTotal(decimal amount, decimal taxRate);
    }

    public class StandardTaxCalculator : ITaxCalculator
    {
        public decimal CalculateTotal(decimal amount, decimal taxRate)
        {
            return amount + (amount * taxRate);
        }
    }

    // --- 2. PERSISTENCE (Data Access Logic) ---
    public interface IOrderRepository
    {
        void Save(Order order);
    }

    public class JsonFileOrderRepository : IOrderRepository
    {
        public void Save(Order order)
        {
            var orderJson = JsonSerializer.Serialize(order);
            File.WriteAllText($"order_{order.Id}.json", orderJson);
            Console.WriteLine($"Saved order {order.Id} to file database.");
        }
    }

    // --- 3. NOTIFICATION (Communication Logic) ---
    public interface INotificationService
    {
        void SendOrderConfirmation(string email, decimal totalAmount);
    }

    public class SmtpNotificationService : INotificationService
    {
        public void SendOrderConfirmation(string email, decimal totalAmount)
        {
            using (var message = new MailMessage("no-reply@mycompany.com", email))
            {
                message.Subject = "Order Processed Successfully";
                message.Body = $"Hi! Your order of {totalAmount:C} has been successfully processed.";
                
                using (var client = new SmtpClient("smtp.mycompany.com"))
                {
                    // Simulated sending
                    Console.WriteLine($"Sending SMTP email to {email} via smtp.mycompany.com...");
                    // client.Send(message);
                }
            }
        }
    }

    // --- 4. ORCHESTRATOR (Order Service) ---
    /// <summary>
    /// SOLUTION OF SRP:
    /// This class has only ONE reason to change: 
    /// If the high-level orchestration workflow of processing an order changes.
    /// It delegates details (Tax logic, DB writes, Email templates) to specialized components.
    /// </summary>
    public class OrderProcessor
    {
        private readonly ITaxCalculator _taxCalculator;
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationService _notificationService;

        public OrderProcessor(
            ITaxCalculator taxCalculator, 
            IOrderRepository orderRepository, 
            INotificationService notificationService)
        {
            _taxCalculator = taxCalculator;
            _orderRepository = orderRepository;
            _notificationService = notificationService;
        }

        public void Process(Order order)
        {
            // Calculate total using tax calculator
            decimal total = _taxCalculator.CalculateTotal(order.Amount, order.TaxRate);
            Console.WriteLine($"Calculated total: {total}");

            // Save order using database repository
            _orderRepository.Save(order);

            // Send confirmation email using notification service
            _notificationService.SendOrderConfirmation(order.CustomerEmail, total);
        }
    }
}
