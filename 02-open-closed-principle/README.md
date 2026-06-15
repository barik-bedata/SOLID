# Open-Closed Principle (OCP)

Software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification.

This means you should be able to extend a class's behavior without modifying its existing source code.

---

## 🛑 The Violation

Take a look at [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/02-open-closed-principle/Violation.cs).
Whenever we add a new payment method (e.g. Bkash), we must edit the `PaymentProcessor` class to add an `else if` check. This violates OCP because the class is modified every time a new feature is added.

## 🟢 The Solution

Take a look at [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/02-open-closed-principle/Solution.cs).
We use the `IPaymentMethod` interface. Adding `BkashPayment` is done by creating a new class, completely leaving the `PaymentProcessor` code untouched.

---

## 💡 Clearing Common Misconceptions & FAQ (প্রশ্ন ও উত্তর)

Here are the answers to your questions regarding OCP:

### ১. প্রশ্ন: কোডে বাগ (Bug) থাকলে কি ক্লাস পরিবর্তন করা যাবে? (Can we modify a class to fix a bug?)

**উত্তর (Yes, absolutely):**
* **OCP মানে এই নয় যে বাগ ফিক্স করা যাবে না।** ওল্ড কোডে কোনো বাগ থাকলে সেটা ফিক্স করার জন্য অবশ্যই আপনি মূল ক্লাসের কোড পরিবর্তন করবেন। 
* OCP-র মূল উদ্দেশ্য হলো **নতুন ফিচার (Feature) বা আচরণ (Behavior)** যোগ করার সময় যেন পুরোনো ও ভালোমতো কাজ করা কোডে হাত দিতে না হয়। বাগ ফিক্সিং হচ্ছে ভুল কোডকে ঠিক করা, কোনো নতুন ফিচার অ্যাড করা নয়। তাই বাগ ফিক্স করার জন্য নিশ্চিন্তে কোড পরিবর্তন করতে পারবেন।

---

### ২. প্রশ্ন: নতুন মেথড অ্যাড করতে গেলে কি ইন্টারফেস পরিবর্তন করতে হবে? (Do we have to change the interface to add a new method?)

**উত্তর:**
যদি আপনি ইন্টারফেসে সরাসরি একটি মেথড যোগ করেন, তবে ওই ইন্টারফেসটি যারা ইমপ্লিমেন্ট করেছে (যেমন `CreditCardPayment`, `PayPalPayment`), তাদের সবার ভেতরেই ওই নতুন মেথডটি জোরপূর্বক ইমপ্লিমেন্ট করতে হবে। এটি ওল্ড কোড ব্রেক করে।

নিচে ডায়াগ্রাম ও বিস্তারিত ব্যাখ্যা দেওয়া হলো:

#### ❌ সমস্যা (The Problem Diagram)
যদি আমরা সরাসরি `IPaymentMethod` ইন্টারফেসে `VerifyOtp()` নামক একটি নতুন মেথড যোগ করি:

```mermaid
classDiagram
    class IPaymentMethod {
        <<interface>>
        +Process(amount)
        +VerifyOtp() <-- [নতুন মেথড যোগ করা হলো]
    }
    class CreditCardPayment {
        +Process(amount)
        +VerifyOtp() <-- [বাধ্যতামূলক ইমপ্লিমেন্ট করতে হবে]
    }
    class PayPalPayment {
        +Process(amount)
        +VerifyOtp() <-- [ভুল! জোর করে ইমপ্লিমেন্ট করতে হচ্ছে (Compiler Error)]
    }
    IPaymentMethod <|.. CreditCardPayment
    IPaymentMethod <|.. PayPalPayment
```

* **কেন সমস্যা?** ক্রেডিট কার্ডের জন্য ওটিপি (OTP) দরকার হলেও পেপ্যালের (PayPal) জন্য ওটিপি দরকার নেই। কিন্তু ইন্টারফেসে মেথড যোগ করায় পেপ্যাল ক্লাসে কম্পাইলার এরর (Compiler Error) দেবে এবং বলবে `VerifyOtp()` ইমপ্লিমেন্ট করো। অর্থাৎ আমাদের পেপ্যাল ক্লাসের সোর্স কোড জোর করে মডিফাই করতে হচ্ছে, যা OCP লঙ্ঘন করে।

---

### 💡 এই সমস্যা এড়ানোর ৩টি উপায় (Solutions):

#### উপায় ১: ইন্টারফেস ইনহেরিটেন্স (Interface Inheritance)
আগের ইন্টারফেসে হাত না দিয়ে, একটি নতুন ইন্টারফেস তৈরি করব যা পুরাতন ইন্টারফেসকে ইনহেরিট (Inherit) করে।

```mermaid
classDiagram
    class IPaymentMethod {
        <<interface>>
        +Process(amount)
    }
    class ISecurePaymentMethod {
        <<interface>>
        +VerifyOtp()
    }
    class CreditCardPayment {
        +Process(amount)
        +VerifyOtp()
    }
    class PayPalPayment {
        +Process(amount)
    }
    IPaymentMethod <|-- ISecurePaymentMethod
    ISecurePaymentMethod <|.. CreditCardPayment
    IPaymentMethod <|.. PayPalPayment
```

* **কেন ভালো?** `PayPalPayment` ক্লাসটি আগের মতোই শুধু `IPaymentMethod` ইমপ্লিমেন্ট করে থাকবে, তাকে টাচও করতে হবে না। আর যে ক্লাসের ওটিপি ভেরিফিকেশন লাগবে (যেমন `CreditCardPayment`), সে নতুন ইন্টারফেস `ISecurePaymentMethod` ইমপ্লিমেন্ট করবে।

---

#### উপায় ২: এক্সটেনশন মেথড (Extension Methods)
ইন্টারফেস বা কনক্রিট ক্লাস কোনো কিছুতেই কোড না লিখে, একদম আলাদা একটি ফাইলে সি-শার্পের Extension Method ব্যবহার করে মেথড যোগ করা।

```mermaid
classDiagram
    class IPaymentMethod {
        <<interface>>
        +Process(amount)
    }
    class PaymentExtensions {
        <<static>>
        +PrintReceipt(IPaymentMethod, amount)
    }
    note for PaymentExtensions "ইন্টারফেসে কোনো কোড এডিট না করেই\nবাহির থেকে নতুন মেথড যোগ করে"
```

* **কোড উদাহরণ:**
```csharp
public static class PaymentExtensions
{
    // this IPaymentMethod ব্যবহার করায় এটি সরাসরি মেথডের মতো কল করা যাবে
    public static void PrintReceipt(this IPaymentMethod paymentMethod, decimal amount)
    {
        Console.WriteLine($"Receipt printed for {amount}");
    }
}
```
* **ব্যবহার:** `paypalPayment.PrintReceipt(500);`
* **কেন ভালো?** `IPaymentMethod` বা এর কোনো ক্লাসে একটি লাইনের কোডও পরিবর্তন করতে হলো না, অথচ আমরা একটি নতুন কাজ/মেথড পেয়ে গেলাম!

---

#### উপায় ৩: ডেকোরেটর প্যাটার্ন (Decorator Pattern)
বিদ্যমান ক্লাসের কোনো কোড না বদলে, তার চারদিকে একটি "র‍্যাপার" বা কভার ক্লাস তৈরি করে নতুন দায়িত্ব যোগ করা।

```mermaid
classDiagram
    class IPaymentMethod {
        <<interface>>
        +Process(amount)
    }
    class PayPalPayment {
        +Process(amount)
    }
    class PaymentWithSmsNotifier {
        -IPaymentMethod _innerPayment
        +Process(amount)
        +SendSmsNotification()
    }
    IPaymentMethod <|.. PayPalPayment
    IPaymentMethod <|.. PaymentWithSmsNotifier
    PayPalPayment <-- PaymentWithSmsNotifier : Wraps
```

* **কেন ভালো?** আমরা পেপ্যাল পেমেন্ট হওয়ার পর একটি SMS নোটিফিকেশন পাঠাতে চাই। আমরা `PayPalPayment` ক্লাসে হাত না দিয়ে `PaymentWithSmsNotifier` নামক একটি নতুন ক্লাস বানালাম, যা ভেতরে অন্য পেমেন্ট মেথডকে রান করে এবং সাথে বাড়তি কাজ (SMS পাঠানো) সম্পন্ন করে।
