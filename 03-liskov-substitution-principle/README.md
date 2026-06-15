# Liskov Substitution Principle (LSP)

Objects of a superclass should be replaceable with objects of its subclasses without affecting the correctness of the program.

সহজ কথায়: **চাইল্ড ক্লাসকে (Child Class) এমনভাবে তৈরি করতে হবে যেন সে তার প্যারেন্ট ক্লাসের (Parent Class) জায়গা পুরোপুরি নিতে পারে, কিন্তু সিস্টেম ক্র্যাশ বা কোনো এরর (Error) না হয়।** প্যারেন্ট ক্লাস যা যা করার প্রতিশ্রুতি দেয়, চাইল্ড ক্লাসকেও হুবহু তা পূরণ করতে হবে।

---

## 🛑 The Violation — [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/03-liskov-substitution-principle/Violation.cs)

ধরুন আমাদের একটি বেস ক্লাস আছে `PaymentMethod`, যার দুটি কাজ আছে: পেমেন্ট প্রসেস করা (`ProcessPayment`) এবং রিফান্ড করা (`Refund`)।

```csharp
public abstract class PaymentMethod
{
    public abstract void ProcessPayment(decimal amount);
    public abstract void Refund(decimal amount);
}

// ১. ক্রেডিট কার্ড রিফান্ড করতে পারে, তাই সমস্যা নেই।
public class CreditCardPayment : PaymentMethod
{
    public override void ProcessPayment(decimal amount) { Console.WriteLine("Card Processed"); }
    public override void Refund(decimal amount) { Console.WriteLine("Card Refunded"); }
}

// ২. ❌ কিন্তু গিফট কার্ড তো রিফান্ড করা যায় না!
public class GiftCardPayment : PaymentMethod
{
    public override void ProcessPayment(decimal amount) { Console.WriteLine("Gift Card Processed"); }
    
    // গিফট কার্ড রিফান্ড হয় না, তাই জোর করে Exception থ্রো করা হলো।
    public override void Refund(decimal amount)
    {
        throw new NotSupportedException("Gift Card cannot be refunded!");
    }
}
```

**সমস্যা কোথায় (LSP Violation)?** 
যখন `RefundProcessor` ক্লাস সব `PaymentMethod` লিস্ট ধরে ধরে `Refund()` কল করবে, তখন `CreditCardPayment` ঠিকঠাক কাজ করবে। কিন্তু যখনই সে `GiftCardPayment` পাবে, তখনই সিস্টেম **Crash** করবে! 

```csharp
public void ProcessRefunds(PaymentMethod[] payments, decimal amount)
{
    foreach (var payment in payments)
    {
        payment.Refund(amount); // GiftCard আসলে এখানে ক্র্যাশ হবে!
    }
}
```

অর্থাৎ, চাইল্ড ক্লাস (`GiftCardPayment`) তার প্যারেন্ট ক্লাসের (`PaymentMethod`) সব আচরণ পালন করতে না পারায় প্যারেন্টের জায়গা নিতে ব্যর্থ হয়েছে। এটিই **LSP Violation**।

---

## 🟢 The Solution — [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/03-liskov-substitution-principle/Solution.cs)

প্যারেন্ট ক্লাসকে এমন কিছু করতে বাধ্য করা যাবে না যা তার সব চাইল্ড করতে পারে না। তাই আমরা কাজগুলোকে আলাদা করে দেব:

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
    public void ProcessPayment(decimal amount) { Console.WriteLine("Card Processed"); }
    public void Refund(decimal amount) { Console.WriteLine("Card Refunded"); }
}

// ৪. গিফট কার্ড শুধু পেমেন্ট পারে, রিফান্ড নয়! তাই সে IRefundable ব্যবহার করবে না।
public class GiftCardPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount) { Console.WriteLine("Gift Card Processed"); }
}
```

এখন `RefundProcessor` ক্লাস শুধুমাত্র `IRefundable` ইন্টারফেস গ্রহণ করবে:

```csharp
public class RefundProcessor
{
    // এখানে ভুল করে GiftCardPayment দেওয়াই যাবে না, কম্পাইলার এরর দেবে!
    public void ProcessRefunds(IRefundable[] refundablePayments, decimal amount)
    {
        foreach (var payment in refundablePayments)
        {
            payment.Refund(amount); // নিশ্চিন্তে রিফান্ড হবে, কোনো ক্র্যাশ নেই!
        }
    }
}
```

**লাভ কী হলো?** 
এখন `RefundProcessor` শুধুমাত্র `IRefundable` ইন্টারফেস গ্রহণ করে। তাই `GiftCardPayment` এখানে ভুল করেও পাস করা যাবে না (কম্পাইল-টাইম এরর)। ফলে সিস্টেম ক্র্যাশ করার কোনো সুযোগ নেই। চাইল্ড ক্লাসগুলো এখন তাদের নিজস্ব আচরণ অনুযায়ী সঠিক ইন্টারফেস ইমপ্লিমেন্ট করছে।
