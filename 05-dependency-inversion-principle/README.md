# Dependency Inversion Principle (DIP)

High-level modules should not depend on low-level modules. Both should depend on abstractions.
Abstractions should not depend on details. Details should depend on abstractions.

সহজ কথায়: **বড় বা মেইন ক্লাসগুলো (High-Level) কখনো ছোট বা সাপোর্টিং ক্লাসগুলোর (Low-Level) উপর সরাসরি নির্ভর করবে না। তারা দুজনেই একটি ইন্টারফেস বা অ্যাবস্ট্রাকশনের (Abstraction) উপর নির্ভর করবে।** এতে কোড "Loose Coupled" বা স্বাধীন হয়।

---

## 🛑 The Violation — [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/05-dependency-inversion-principle/Violation.cs)

ধরুন আমাদের ই-কমার্স সিস্টেমে একটি মেইন ক্লাস আছে `OrderProcessor` (High-Level)। এর কাজ হলো অর্ডার প্রসেস শেষে কাস্টমারকে ইমেইল পাঠানো। ইমেইল পাঠানোর কাজ করে `EmailSender` (Low-Level)।

```csharp
public class EmailSender
{
    public void SendEmail(string message) { Console.WriteLine("Sending Email..."); }
}

public class OrderProcessor
{
    private readonly EmailSender _emailSender;

    public OrderProcessor()
    {
        // ❌ DIP Violation: হাই-লেভেল ক্লাস সরাসরি একটি লো-লেভেল ক্লাসের অবজেক্ট তৈরি করছে!
        _emailSender = new EmailSender();
    }

    public void ProcessOrder()
    {
        _emailSender.SendEmail("Your order has been confirmed.");
    }
}
```

**সমস্যা কোথায় (DIP Violation)?** 
`OrderProcessor` সরাসরি `EmailSender`-এর সাথে আঠার মতো লেগে আছে (Tightly Coupled)। কালকে যদি ক্লায়েন্ট বলে, *"ইমেইলের বদলে SMS পাঠাতে হবে"*, তখন আপনাকে বাধ্য হয়ে পুরো `OrderProcessor` ক্লাসের ভেতরের কোড পরিবর্তন করতে হবে। এটি OCP (Open-Closed Principle)-কেও ভঙ্গ করে। 

---

## 🟢 The Solution — [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/05-dependency-inversion-principle/Solution.cs)

হাই-লেভেল ক্লাসকে লো-লেভেল ক্লাস থেকে মুক্ত (Decoupled) করতে হবে। এর জন্য আমরা একটি ইন্টারফেস (Abstraction) তৈরি করব।

```csharp
// ১. Abstraction (ইন্টারফেস)
public interface INotificationService
{
    void SendMessage(string message);
}

// ২. Low-Level Module (ইন্টারফেস মেনে চলছে)
public class EmailSender : INotificationService
{
    public void SendMessage(string message) { Console.WriteLine("Sending Email..."); }
}

public class SmsSender : INotificationService
{
    public void SendMessage(string message) { Console.WriteLine("Sending SMS..."); }
}
```

এখন আমাদের `OrderProcessor` (High-Level) আর `EmailSender`-কে চিনবে না, সে শুধু `INotificationService`-কে চিনবে। এবং এই সার্ভিসটি সে বাইরে থেকে (Constructor-এর মাধ্যমে) গ্রহণ করবে। একেই বলে **Dependency Injection**:

```csharp
public class OrderProcessor
{
    private readonly INotificationService _notificationService;

    // ✅ Constructor Injection: Dependency বাইরে থেকে পাস করা হচ্ছে।
    public OrderProcessor(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void ProcessOrder()
    {
        _notificationService.SendMessage("Your order has been confirmed.");
    }
}
```

**লাভ কী হলো?** 
এখন আপনি `OrderProcessor`-কে একটুও পরিবর্তন না করে যেকোনো মাধ্যম দিয়ে মেসেজ পাঠাতে পারবেন!

```csharp
// ইমেইল পাঠাতে চাইলে:
OrderProcessor emailProcessor = new OrderProcessor(new EmailSender());

// SMS পাঠাতে চাইলে (OrderProcessor এ কোনো কোড চেঞ্জ করা লাগেনি!):
OrderProcessor smsProcessor = new OrderProcessor(new SmsSender());
```
এটাই হলো **Dependency Inversion Principle**-এর জাদুকরী ক্ষমতা! এটি আপনার কোডকে ফ্লেক্সিবল, টেস্টেবল এবং দীর্ঘস্থায়ী করে তোলে।
