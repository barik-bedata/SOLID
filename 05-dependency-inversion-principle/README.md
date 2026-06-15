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

---

## 🔗 DIP এবং Coupling-এর সম্পর্ক (Tight vs Loose Coupling)

DIP (Dependency Inversion Principle)-এর সাথে **Coupling**-এর সম্পর্ক একেবারে জন্মগত! বলতে গেলে, কোডকে "Coupling" এর অভিশাপ থেকে বাঁচাতেই DIP এর জন্ম।

### ১. Tight Coupling (আঠার মতো লেগে থাকা)
যখন একটি ক্লাস সরাসরি `new` কিওয়ার্ড ব্যবহার করে অন্য একটি ক্লাসের অবজেক্ট তৈরি করে, তখন তারা একে অপরের সাথে আঠার মতো লেগে যায়। একেই বলে **Tight Coupling** (বা কড়া নির্ভরতা)।

যেমন আমাদের `Violation.cs` ফাইলে:
```csharp
public class OrderProcessor
{
    private readonly EmailSender _emailSender = new EmailSender(); // ❌ Tight Coupling
}
```
এখানে `OrderProcessor` ক্লাসটি `EmailSender`-এর সাথে টাইট কাপলড। সমস্যাটা কী? 
যদি কালকে ইমেইলের বদলে SMS পাঠাতে হয়, তখন আপনাকে বাধ্য হয়ে `OrderProcessor` ক্লাসের ভেতরের কোড কাটাকাটি করতে হবে। এক ক্লাসের পরিবর্তনের কারণে অন্য ক্লাসকেও পাল্টাতে হচ্ছে—এটাই টাইট কাপলিংয়ের প্রধান সমস্যা।

### ২. Loose Coupling (স্বাধীন করে দেওয়া)
সফটওয়্যার ইঞ্জিনিয়ারিংয়ের সবচেয়ে বড় রুলস হলো: **কোডকে সবসময় Loose Coupled হতে হবে।** অর্থাৎ, এক ক্লাসের সাথে আরেক ক্লাসের সম্পর্ক থাকবে ঠিকই, কিন্তু তারা একে অপরের উপর অন্ধভাবে নির্ভরশীল হবে না। 

আর এই **Loose Coupling** অর্জন করার সবচেয়ে বড় হাতিয়ারই হলো **DIP**!
DIP বলে, সরাসরি একে অপরের উপর নির্ভর না করে, মাঝখানে একটি "চুক্তি" বা "ইন্টারফেস" (Abstraction) বসিয়ে দাও।

যেমন আমাদের `Solution.cs` ফাইলে:
```csharp
public class OrderProcessor
{
    // ✅ Loose Coupling
    private readonly INotificationService _notificationService; 

    public OrderProcessor(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
}
```
এখানে `OrderProcessor` আর কোনো নির্দিষ্ট ক্লাসকে চেনে না। সে শুধু `INotificationService` ইন্টারফেসকে চেনে। 

এখন আপনি বাইরে থেকে তাকে `EmailSender` দেন, আর `SmsSender` দেন—তার কিছুই যায় আসে না। তার ভেতরে কোনো কোডই চেঞ্জ করতে হবে না! `OrderProcessor` এখন পুরোপুরি স্বাধীন বা **Loose Coupled**।

**সারসংক্ষেপ:** 
**Tight Coupling** হলো একটি রোগের নাম, আর **DIP** হলো সেই রোগের ঔষধ, যা খাইয়ে কোডকে **Loose Coupled** (সুস্থ) করা হয়!
