# Open-Closed Principle (OCP)

Software entities (classes, modules, functions, etc.) should be **open for extension, but closed for modification**.

সহজ কথায়: নতুন ফিচার যোগ করার সময় পুরাতন কোডে হাত দিতে হবে না। নতুন কোড লিখে পুরাতন কোডকে extend করব।

---

## 🛑 The Violation — [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/02-open-closed-principle/Violation.cs)

এখানে প্রতিটি পেমেন্ট ক্লাসের নিজস্ব আলাদা মেথড নাম আছে। `PaymentProcessor` ক্লাসকে `is` কিওয়ার্ড দিয়ে টাইপ চেক করে বুঝতে হচ্ছে কোন ক্লাস এসেছে:

```csharp
// প্রতিটি ক্লাসের আলাদা আলাদা মেথড নাম
public class CreditCardPayment
{
    public void PayWithCard(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of {amount}");
    }
}

public class PayPalPayment
{
    public void PayWithPayPal(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of {amount}");
    }
}

// PaymentProcessor কে টাইপ চেক করে প্রতিটি ক্লাসের মেথড আলাদাভাবে কল করতে হচ্ছে
public class PaymentProcessor
{
    public void ProcessPayment(object payment, decimal amount)
    {
        if (payment is CreditCardPayment cardPayment)
        {
            cardPayment.PayWithCard(amount);
        }
        else if (payment is PayPalPayment payPalPayment)
        {
            payPalPayment.PayWithPayPal(amount);
        }
        // ভবিষ্যতে Bkash যোগ করতে গেলে এখানে আরেকটি else if লিখতে হবে!
        // else if (payment is BkashPayment bkash) { bkash.PayWithBkash(amount); }
    }
}
```

**সমস্যা কোথায়?** নতুন পেমেন্ট মেথড (যেমন `BkashPayment`) যোগ করতে গেলে প্রতিবার `PaymentProcessor` ক্লাসের কোড এডিট করতে হবে। এটি OCP লঙ্ঘন, কারণ ক্লাসটি modification-এর জন্য বন্ধ (closed) নয়।

---

## 🟢 The Solution — [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/02-open-closed-principle/Solution.cs)

আমরা একটি কমন `IPaymentMethod` ইন্টারফেস তৈরি করি, যেখানে সবার একটাই মেথড: `Process(amount)`। এর ফলে `PaymentProcessor` কে কোন ক্লাস এসেছে তা জানতে হয় না:

```csharp
// সবার জন্য একটি কমন ইন্টারফেস
public interface IPaymentMethod
{
    void Process(decimal amount);
}

// প্রতিটি পেমেন্ট ক্লাস ইন্টারফেস ইমপ্লিমেন্ট করে
public class CreditCardPayment : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of {amount}");
    }
}

public class PayPalPayment : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of {amount}");
    }
}

// নতুন Bkash যোগ করতে শুধু নতুন ক্লাস বানালেই হলো!
// PaymentProcessor এর কোড স্পর্শ করতে হলো না!
public class BkashPayment : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing Bkash payment of {amount}");
    }
}

// PaymentProcessor এখন শুধু ইন্টারফেস দিয়ে কাজ করে
// কোন ক্লাস এসেছে সেটা জানার দরকার নেই
public class PaymentProcessor
{
    public void ProcessPayment(IPaymentMethod paymentMethod, decimal amount)
    {
        paymentMethod.Process(amount); // ব্যস! কোনো if/else নেই!
    }
}
```

**লাভ কী হলো?** নতুন `BkashPayment` যোগ করতে শুধু একটি নতুন ক্লাস তৈরি করলেই হলো। `PaymentProcessor` এর এক লাইনও এডিট করতে হলো না। এটিই হলো OCP — **Open for Extension, Closed for Modification**।

---

## 💡 প্রশ্ন ও উত্তর (FAQ)

### ১. কোডে বাগ (Bug) থাকলে কি ক্লাস পরিবর্তন করা যাবে?

**হ্যাঁ, অবশ্যই পরিবর্তন করা যাবে।**

OCP মানে এই নয় যে একবার কোড লেখা হলে সেটা আর কখনো ছোঁয়া যাবে না। OCP শুধুমাত্র বলে: **নতুন ফিচার বা নতুন আচরণ (Behavior)** যোগ করার সময় পুরাতন কোড এডিট করো না।

কিন্তু **বাগ ফিক্সিং** এবং **নতুন ফিচার যোগ করা** দুটি সম্পূর্ণ আলাদা জিনিস:

* **বাগ ফিক্সিং** = পুরাতন কোড যেভাবে কাজ করার কথা ছিল সেভাবে কাজ করছে না, সেটা ঠিক করা। যেমন:
  - `Process()` মেথডে ট্যাক্স হিসাব ভুল হচ্ছিল (১০% এর জায়গায় ১০০% চার্জ হচ্ছিল), সেটা ঠিক করা।
  - কোনো মেথডে `NullReferenceException` আসছিল, সেটা ফিক্স করা।
  - একটি `if` কন্ডিশনে ভুল লজিক ছিল, সেটা সংশোধন করা।

* **নতুন ফিচার যোগ করা** = আগে যা ছিল না এমন কিছু নতুন যুক্ত করা। যেমন:
  - পেমেন্ট সিস্টেমে নতুন বিকাশ (Bkash) পেমেন্ট মেথড যোগ করা।
  - পেমেন্টের পর SMS নোটিফিকেশন পাঠানোর ফিচার যোগ করা।
  - OTP ভেরিফিকেশন ফিচার যোগ করা।

**সারকথা:** বাগ ফিক্সিং এর জন্য মূল ক্লাসের কোড পরিবর্তন করা OCP লঙ্ঘন নয়। কিন্তু নতুন ফিচার (যেমন নতুন পেমেন্ট মেথড, নতুন নোটিফিকেশন সিস্টেম) যোগ করতে গিয়ে যদি পুরাতন ক্লাসের কোড এডিট করতে হয়, তাহলে সেটা OCP লঙ্ঘন।

---

### ২. বিদ্যমান ইন্টারফেসে সরাসরি নতুন মেথড সিগনেচার যোগ করা কি OCP লঙ্ঘন?

**হ্যাঁ, এটি OCP লঙ্ঘন।** ধরুন আমরা `IPaymentMethod` ইন্টারফেসে সরাসরি `VerifyOtp()` মেথড যোগ করলাম:

```csharp
// ❌ সমস্যা: সরাসরি মূল ইন্টারফেস এডিট করা হচ্ছে
public interface IPaymentMethod
{
    void Process(decimal amount);
    void VerifyOtp(); // <-- নতুন মেথড সরাসরি যোগ করা হলো
}

// এখন PayPalPayment ক্লাসে কম্পাইলার এরর আসবে!
// কারণ PayPalPayment এ VerifyOtp() ইমপ্লিমেন্ট করা নেই।
// আমাদের বাধ্য হয়ে PayPalPayment এর কোডও এডিট করতে হবে:
public class PayPalPayment : IPaymentMethod
{
    public void Process(decimal amount) { ... }
    public void VerifyOtp() { throw new NotImplementedException(); } // জোর করে লিখতে হচ্ছে!
}
```

**কেন সমস্যা?** পেপ্যালের ওটিপি ভেরিফিকেশন দরকার নেই, কিন্তু ইন্টারফেসে মেথড যোগ করায় পেপ্যাল ক্লাসের কোডও জোর করে মডিফাই করতে হচ্ছে।

---

### ৩. তাহলে মূল ইন্টারফেস এডিট না করে নতুন মেথড কীভাবে যোগ করব?

৩টি উপায় আছে:

#### উপায় ১: ইন্টারফেস ইনহেরিটেন্স (Recommended ✅)

মূল ইন্টারফেসে হাত না দিয়ে নতুন একটি ইন্টারফেস বানাব যা পুরাতনটিকে ইনহেরিট করে:

```csharp
// মূল ইন্টারফেস — একদম অপরিবর্তিত থাকবে
public interface IPaymentMethod
{
    void Process(decimal amount);
}

// নতুন ইন্টারফেস — মূল ইন্টারফেসকে ইনহেরিট করে
public interface ISecurePaymentMethod : IPaymentMethod
{
    void VerifyOtp();
}

// PayPalPayment — আগের মতোই আছে, কোনো পরিবর্তন নেই
public class PayPalPayment : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of {amount}");
    }
}

// CreditCardPayment — নতুন ইন্টারফেস ব্যবহার করছে কারণ এর OTP দরকার
public class CreditCardPayment : ISecurePaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of {amount}");
    }

    public void VerifyOtp()
    {
        Console.WriteLine("OTP verified for Credit Card.");
    }
}
```

**লাভ:** মূল `IPaymentMethod` এডিট হয়নি। `PayPalPayment` এডিট হয়নি। শুধু যার ওটিপি দরকার সেই `CreditCardPayment` নতুন ইন্টারফেস ব্যবহার করছে।

---

#### উপায় ২: এক্সটেনশন মেথড (Extension Methods)

কোনো ইন্টারফেস বা ক্লাস এডিট না করে, সম্পূর্ণ আলাদা ফাইলে নতুন মেথড যোগ করা:

```csharp
// IPaymentMethod এবং এর সব ক্লাস — সব অপরিবর্তিত থাকবে

// সম্পূর্ণ আলাদা একটি ফাইলে এটি লিখব:
public static class PaymentExtensions
{
    // 'this' কিওয়ার্ড ব্যবহারে এটি IPaymentMethod এর নিজস্ব মেথডের মতো কাজ করবে
    public static void PrintReceipt(this IPaymentMethod paymentMethod, decimal amount)
    {
        Console.WriteLine($"--- RECEIPT ---");
        Console.WriteLine($"Amount: {amount:C}");
        Console.WriteLine($"---------------");
    }
}

// ব্যবহার:
// PayPalPayment paypal = new PayPalPayment();
// paypal.Process(500);
// paypal.PrintReceipt(500);  <-- ইন্টারফেসে না থাকলেও সরাসরি কল করা যাচ্ছে!
```

**লাভ:** `IPaymentMethod` বা কোনো পেমেন্ট ক্লাসের এক লাইনও এডিট হয়নি, অথচ নতুন মেথড পেয়ে গেলাম!

---

#### উপায় ৩: ডেকোরেটর প্যাটার্ন (Decorator Pattern)

বিদ্যমান ক্লাসের বাইরে একটি "র‍্যাপার" ক্লাস তৈরি করে নতুন কাজ যুক্ত করা:

```csharp
// PayPalPayment — আগের মতোই, কোনো পরিবর্তন নেই

// নতুন র‍্যাপার ক্লাস — পেমেন্টের পর SMS পাঠাবে
public class PaymentWithSmsDecorator : IPaymentMethod
{
    private readonly IPaymentMethod _innerPayment; // ভেতরে আসল পেমেন্ট মেথড রাখা হবে

    public PaymentWithSmsDecorator(IPaymentMethod innerPayment)
    {
        _innerPayment = innerPayment;
    }

    public void Process(decimal amount)
    {
        _innerPayment.Process(amount);  // ১. আগে আসল পেমেন্ট হবে
        SendSms(amount);                // ২. তারপর SMS পাঠাবে
    }

    private void SendSms(decimal amount)
    {
        Console.WriteLine($"[SMS] Payment of {amount:C} successful!");
    }
}

// ব্যবহার:
// IPaymentMethod paypal = new PayPalPayment();
// IPaymentMethod paypalWithSms = new PaymentWithSmsDecorator(paypal);
// paypalWithSms.Process(500);
// Output:
//   Processing PayPal payment of 500
//   [SMS] Payment of ৳500.00 successful!
```

**লাভ:** `PayPalPayment` ক্লাসে হাত না দিয়েই পেমেন্টের পর SMS পাঠানোর ফিচার যোগ করা হলো!

---

### ৪. যদি কোনো ইন্টারফেস কেবল একটি ক্লাসেই ইমপ্লিমেন্ট করা থাকে (1-to-1), তবে কি সরাসরি ইন্টারফেসে মেথড যোগ করা যাবে?

**উত্তর:**
* **তাত্ত্বিক দৃষ্টিকোণ থেকে:** হ্যাঁ, ওল্ড ফাইল এডিট করার কারণে এটি তাত্ত্বিকভাবে মডিফিকেশন।
* **বাস্তব প্র্যাক্টিক্যাল দৃষ্টিকোণ থেকে:** এটি করা সম্পূর্ণ ঠিক আছে। যেহেতু ইন্টারফেসটি কেবল একটি ক্লাসই ইমপ্লিমেন্ট করেছে (যেমন `IUserService` ও `UserService`), সেহেতু অন্য কোনো ক্লাস ব্রেক হওয়ার সম্ভাবনা নেই। তাই বাস্তব প্রজেক্টে ১-টু-১ সম্পর্কের ক্ষেত্রে সরাসরি মেথড যোগ করা একটি সাধারণ ও সঠিক প্র্যাকটিস।
