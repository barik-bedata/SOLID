# Liskov Substitution Principle (LSP)

Objects of a superclass should be replaceable with objects of its subclasses without affecting the correctness of the program.

সহজ কথায়: **চাইল্ড ক্লাসকে (Child Class) এমনভাবে তৈরি করতে হবে যেন সে তার বেস ইন্টারফেস বা প্যারেন্ট ক্লাসের (Parent Class) জায়গা পুরোপুরি নিতে পারে, কিন্তু সিস্টেম ক্র্যাশ বা কোনো এরর (Error) না হয়।** বেস ইন্টারফেস যা যা করার প্রতিশ্রুতি দেয়, চাইল্ড ক্লাসকেও হুবহু তা পূরণ করতে হবে।

---

## 🛑 The Violation — [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/03-liskov-substitution-principle/Violation.cs)

ধরুন আমাদের একটি বেস ইন্টারফেস আছে `IPaymentMethod`, যার দুটি কাজ আছে: পেমেন্ট প্রসেস করা (`ProcessPayment`) এবং রিফান্ড করা (`Refund`)।

```csharp
public interface IPaymentMethod
{
    void ProcessPayment(decimal amount);
    void Refund(decimal amount);
}

// ১. ক্রেডিট কার্ড রিফান্ড করতে পারে, তাই সমস্যা নেই।
public class CreditCardPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount) => Console.WriteLine("Card Processed");
    public void Refund(decimal amount) => Console.WriteLine("Card Refunded");
}

// ২. ❌ কিন্তু গিফট কার্ড তো রিফান্ড করা যায় না!
public class GiftCardPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount) => Console.WriteLine("Gift Card Processed");
    
    // গিফট কার্ড রিফান্ড হয় না, তাই জোর করে Exception থ্রো করা হলো।
    public void Refund(decimal amount)
    {
        throw new NotSupportedException("Gift Card cannot be refunded!");
    }
}
```

### 💥 সমস্যা কোথায় (LSP Violation)? 

লুপ ব্যবহার না করে সরাসরি মেথড কলের মাধ্যমে সমস্যাটি দেখি। আমাদের একটি `RefundService` আছে যা যেকোনো `IPaymentMethod` গ্রহণ করে:

```csharp
public class RefundService
{
    public void ExecuteRefund(IPaymentMethod payment, decimal amount)
    {
        // যদি GiftCardPayment পাস করা হয়, তাহলে এখানে অ্যাপ্লিকেশন ক্র্যাশ করবে!
        payment.Refund(amount);
    }
}

// Usage:
RefundService refundService = new RefundService();

IPaymentMethod card = new CreditCardPayment();
refundService.ExecuteRefund(card, 100); // ✅ ঠিকঠাক কাজ করবে

IPaymentMethod giftCard = new GiftCardPayment();
refundService.ExecuteRefund(giftCard, 100); // 💥 Crash! NotSupportedException
```

**LSP Violation কেন?** `ExecuteRefund` মেথডটি আশা করেছিল যেকোনো `IPaymentMethod`-ই রিফান্ড করতে পারবে। কিন্তু `GiftCardPayment` তার বেস ইন্টারফেসের (`IPaymentMethod`) আচরণ পালন করতে না পারায় সিস্টেম ক্র্যাশ করেছে। এটিই **Liskov Substitution Principle (LSP)** এর স্পষ্ট লঙ্ঘন।

---

## 🟢 The Solution — [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/03-liskov-substitution-principle/Solution.cs)

ইন্টারফেসকে এমন কিছু করতে বাধ্য করা যাবে না যা তার সব ইমপ্লিমেন্টিং ক্লাস করতে পারে না। তাই আমরা কাজগুলোকে আলাদা করে দেব:

```csharp
// ১. সাধারণ পেমেন্ট (যা সবাই করতে পারে)
public interface IPaymentMethod
{
    void ProcessPayment(decimal amount);
}

// ২. রিফান্ড (যা শুধু রিফান্ডেবল মেথডগুলো করবে)
public interface IRefundable
{
    void Refund(decimal amount);
}

// ৩. ক্রেডিট কার্ড দুটোই পারে
public class CreditCardPayment : IPaymentMethod, IRefundable
{
    public void ProcessPayment(decimal amount) => Console.WriteLine("Card Processed");
    public void Refund(decimal amount) => Console.WriteLine("Card Refunded");
}

// ৪. গিফট কার্ড শুধু পেমেন্ট পারে, রিফান্ড নয়! তাই সে IRefundable ব্যবহার করবে না।
public class GiftCardPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount) => Console.WriteLine("Gift Card Processed");
}
```

এখন `RefundService` ক্লাস শুধুমাত্র `IRefundable` ইন্টারফেস গ্রহণ করবে:

```csharp
public class RefundService
{
    // ✅ এখন এটি শুধুমাত্র IRefundable ইন্টারফেস গ্রহণ করে
    public void ExecuteRefund(IRefundable payment, decimal amount)
    {
        payment.Refund(amount);
    }
}

// Usage:
RefundService refundService = new RefundService();

CreditCardPayment card = new CreditCardPayment();
refundService.ExecuteRefund(card, 100); // ✅ ঠিকঠাক কাজ করবে

GiftCardPayment giftCard = new GiftCardPayment();
// refundService.ExecuteRefund(giftCard, 100); // 🚫 Compiler Error! Safe!
```

**লাভ কী হলো?** 
এখন `RefundService` শুধুমাত্র `IRefundable` ইন্টারফেস গ্রহণ করে। তাই `GiftCardPayment` এখানে ভুল করেও পাস করা যাবে না (কম্পাইল-টাইম এরর)। ফলে লুপ হোক বা সরাসরি কল, সিস্টেম ক্র্যাশ করার কোনো সুযোগ নেই। ইমপ্লিমেন্টিং ক্লাসগুলো এখন তাদের নিজস্ব আচরণ অনুযায়ী সঠিক ইন্টারফেস ব্যবহার করছে।

---

## 💡 প্রশ্ন ও উত্তর (FAQ)

### "Parent should be replaceable by child" — এর মানে কী?

খুব সহজ কথায় বললে:
* **Parent (Parent Type):** ইন্টারফেস বা বেস ক্লাস (যেমন `IPaymentMethod`) হলো একটি **"ফাঁকা বাক্স"** বা **"লেবেল"**।
* **Child (Child Type):** যে ক্লাসটি ঐ ইন্টারফেস ইমপ্লিমেন্ট করে (যেমন `CreditCardPayment` বা `GiftCardPayment`), সেটি হলো আসল কাজ করার **"মেশিন"**।

যখন আমরা লিখি:
```csharp
IPaymentMethod pay = new CreditCardPayment();
```
এর মানে হলো: **"আমাকে IPaymentMethod নামের একটি ফাঁকা বাক্স দাও, আর সেই বাক্সের ভেতরে আসল CreditCardPayment মেশিনটি ঢুকিয়ে দাও।"**

এখানে **Parent (`IPaymentMethod`) এর জায়গাটি দখল করেছে বা রিপ্লেস (Replace) করেছে Child (`CreditCardPayment`)।** 

**LSP এর মূল নিয়ম হলো:**
এই `IPaymentMethod` বাক্সে আপনি যেই Child মেশিনকেই রাখেন না কেন, বাক্সের বাটন (যেমন `ProcessPayment`) চাপলে তা ঠিকঠাক কাজ করতে হবে। কোনোভাবেই যেন সিস্টেম ক্র্যাশ না করে। যদি `GiftCardPayment` কে ঐ বাক্সে রাখলে সিস্টেম ক্র্যাশ করে (যেমনটা আমাদের Violation ফাইলে `Refund` কল করলে হয়েছিল), তার মানে ওই Child তার Parent-এর জায়গা নেওয়ার যোগ্য নয়! এটাই হলো LSP Violation।
