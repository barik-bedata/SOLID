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

### ২. প্রশ্ন: ভবিষ্যতে নতুন মেথড অ্যাড করতে হলে ইন্টারফেস কি পরিবর্তন করতেই হবে? (Do we have to change the interface to add a new method?)

**উত্তর:**
যদি আপনি ইন্টারফেসে সরাসরি একটি মেথড যোগ করেন, তবে ওই ইন্টারফেসটি যারা ইমপ্লিমেন্ট করেছে (যেমন `CreditCardPayment`, `PayPalPayment`), তাদের সবার ভেতরেই ওই নতুন মেথডটি জোরপূর্বক ইমপ্লিমেন্ট করতে হবে। এটি OCP-র লঙ্ঘন (এবং **ISP - Interface Segregation Principle** এরও লঙ্ঘন)।

এটি এড়ানোর জন্য বেশ কিছু চমৎকার উপায় বা প্যাটার্ন আছে:

#### ক) ইন্টারফেস ইনহেরিটেন্স (Interface Inheritance / Extension)
আপনি মূল ইন্টারফেসে হাত না দিয়ে একটি নতুন ইন্টারফেস তৈরি করতে পারেন যা আগের ইন্টারফেসটিকে ইনহেরিট করে।
```csharp
public interface IPaymentMethod
{
    void Process(decimal amount);
}

// নতুন ফিচার বা মেথড যোগ করার জন্য নতুন ইন্টারফেস
public interface ISecurePaymentMethod : IPaymentMethod
{
    void AuthenticateTwoFactor();
}
```
এর ফলে পুরোনো ক্লাসগুলো আগের মতোই থাকবে, আর নতুন যে ক্লাসটির এই মেথড দরকার সে কেবল `ISecurePaymentMethod` ইমপ্লিমেন্ট করবে।

#### খ) এক্সটেনশন মেথড (Extension Methods in C#)
ক্লাস বা ইন্টারফেসকে এডিট না করে নতুন মেথড যোগ করার জন্য সি-শার্পে Extension Methods ব্যবহার করা যায়।
```csharp
public static class PaymentExtensions
{
    public static void PrintReceipt(this IPaymentMethod paymentMethod, decimal amount)
    {
        Console.WriteLine($"Receipt for payment of {amount}");
    }
}
```
এখন আপনি যেকোনো `IPaymentMethod` অবজেক্টের ওপর সরাসরি `.PrintReceipt(100)` মেথড কল করতে পারবেন, কিন্তু মূল ইন্টারফেসে কোনো পরিবর্তন করতে হবে না।

#### গ) ডেকোরেটর প্যাটার্ন (Decorator Pattern)
কোনো ক্লাসের কোড পরিবর্তন না করে তার চারপাশে একটি "র‍্যাপার" (Wrapper) তৈরি করে নতুন আচরণ যোগ করার জন্য ডেকোরেটর প্যাটার্ন ব্যবহার করা হয়। (যেমন: পেমেন্ট করার পর অতিরিক্ত হিসেবে একটি SMS নোটিফিকেশন পাঠানোর মেথড যোগ করা)।
