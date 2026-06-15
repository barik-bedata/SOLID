# গল্পে গল্পে Liskov Substitution Principle (LSP)

LSP শেখার সময় সবার মনেই একটি খুব লজিক্যাল প্রশ্ন আসে: 
*"Parent এর জায়গায় Child কে বসালাম, কোড তো রান হলো এবং সে একটি Exception থ্রো করল। Exception থ্রো করাটাও তো কোডের একটি কাজ! তাহলে এটাকে আমরা Violation বা নিয়ম ভঙ্গ করা কেন বলছি?"*

এই প্রশ্নের উত্তর লুকিয়ে আছে **"Contract" বা "চুক্তি"** নামক একটি কনসেপ্টের মধ্যে। চলুন একটি গল্পের মাধ্যমে বিষয়টা পরিষ্কার করি।

---

## 🤝 চুক্তি বা প্রমিস (The Contract)

যখন আমরা `IPaymentMethod` নামে একটি ইন্টারফেস বানাই এবং তার ভেতরে `Refund()` মেথড রাখি, তখন আমরা পুরো প্রোগ্রামিং সিস্টেমকে একটি **চুক্তি (Promise)** দিচ্ছি:

> *"যেই ক্লাস এই `IPaymentMethod` নামের বাক্সটি ব্যবহার করবে, সে নিশ্চিন্ত থাকতে পারে যে— এই বাক্সের ভেতরে থাকা যেকোনো জিনিসই সফলভাবে রিফান্ড করতে পারবে।"*

আমাদের `RefundService` ক্লাসটি ওই চুক্তির ওপর ১০০% বিশ্বাস রাখে। সে জানে না বাক্সের ভেতরে ক্রেডিট কার্ড আছে নাকি গিফট কার্ড। সে চোখ বন্ধ করে বাক্সের `Refund()` বাটনে চাপ দেয়।

---

## 🚗 লাইসেন্স এবং দুর্ঘটনার গল্প

ধরে নিন, আপনার কাছে একটি **"গাড়ি চালানোর লাইসেন্স"** (এটি হলো আমাদের Parent বা `IPaymentMethod`) আছে। এই লাইসেন্স একটি চুক্তি বা প্রমিস যে— আপনি রাস্তায় নিরাপদে গাড়ি চালাতে পারেন।

এখন আপনাকে রাস্তায় নামানো হলো:
১. আমি যদি আপনাকে একটি **প্রাইভেট কার** (`CreditCardPayment`) চালাতে দিই, আপনি সুন্দরভাবে চালিয়ে দিলেন। কোনো সমস্যা নেই! (LSP Followed)
২. কিন্তু আমি যদি আপনাকে একটি বিশাল **ট্রাক** (`GiftCardPayment`) চালাতে দিই, আর আপনি ট্রাক নিয়ে সোজাসুজি একটা দেয়ালে ধাক্কা মেরে অ্যাক্সিডেন্ট করে বসেন!

এখন পুলিশ এসে ধরলে আপনি কি বলবেন— *"অ্যাক্সিডেন্ট হলেও তো গাড়ি চলেছে, তাহলে লাইসেন্সের ভায়োলেশন হবে কেন?"*
**অবশ্যই না!** 

কারণ লাইসেন্স (Parent) বলেছিল আপনি গাড়ি **নিরাপদে** চালাতে পারবেন। দেয়ালে ধাক্কা মেরে গাড়ি ভেঙে ফেলা (Exception Throw করা) আপনার চুক্তির অংশ ছিল না। অর্থাৎ, আপনি ওই লাইসেন্সটি ধারণ করার যোগ্য ছিলেন পরীক্ষিতভাবে!

---

## 💥 Exception থ্রো করলে আসল সমস্যা কী?

প্রোগ্রামিংয়ে `Exception` থ্রো করা মানে "কাজ করা" নয়, বরং এর মানে হলো— **"আমি কাজটা করতে পারছি না, প্রোগ্রাম এখনই বন্ধ করে দাও!"**

যখন `GiftCardPayment` রিফান্ড করতে না পেরে `NotSupportedException` থ্রো করে, তখন সফটওয়্যারটি (যেমন কোনো মোবাইল অ্যাপ বা ওয়েবসাইট) ইউজারকে এরর পেজ দেখিয়ে সাথে সাথে **Crash বা বন্ধ হয়ে যায়**! সফটওয়্যার ক্র্যাশ করা কোনো "কাজ করা" নয়, এটি একটি মারাত্মক বাগ (Bug), যা কোম্পানির আর্থিক ক্ষতির কারণ হতে পারে।

**LSP (Liskov Substitution Principle) ঠিক এই জায়গাতেই বাধা দেয়।**
LSP বলে: 
> *"তুমি যদি রিফান্ড করতেই না পারো, তাহলে তুমি কেন এমন একটা বক্সে (`IPaymentMethod`) ঢুকে বসে আছো, যে বক্সটা দাবি করে যে সে রিফান্ড করতে পারে? তুমি মিথ্যা চুক্তি করেছো!"*

### 🟢 সমাধান কী?
সমাধান হলো: যে রিফান্ড করতে পারে না, তাকে এমন একটি বক্সে (`IRefundable`) রাখাই যাবে না। তাহলে কেউ ভুল করেও তার রিফান্ড বাটনে চাপ দেবে না, আর সফটওয়্যারটিও কখনো ক্র্যাশ করবে প্রতিনিয়ত। 

এটাই হলো **Liskov Substitution Principle**-এর আসল সৌন্দর্য!

---

## 🎭 Major Behavior Change (আরেক ধরনের ভায়োলেশন)

আমরা এতক্ষণ দেখলাম Exception থ্রো করে কীভাবে কোড ক্র্যাশ করে। কিন্তু রিয়েল-ওয়ার্ল্ড প্রজেক্টে LSP-এর সবচেয়ে ভয়ানক ভায়োলেশন হলো— **কোড ক্র্যাশ করে না, কিন্তু কোডের আসল উদ্দেশ্য বা আচরণ (Behavior) পুরোপুরি বদলে যায়!** 

চলুন ই-কমার্সের একটি **রিয়েল-ওয়ার্ল্ড বিলিং সিস্টেম** দিয়ে বুঝি। (বোরিং জ্যামিতির উদাহরণ বাদ!)

ধরুন, আপনার একটি ইন্টারফেস আছে `IDiscountCalculator`, যার কাজ হলো কাস্টমার কত টাকা ডিসকাউন্ট পাবে তা হিসাব করা।
```csharp
public interface IDiscountCalculator
{
    // Parent এর চুক্তি (Contract): "আমি সবসময় একটি পজিটিভ (Positive) ডিসকাউন্ট অ্যামাউন্ট দেব, 
    // যা মোট বিল থেকে মাইনাস (Subtract) হবে।"
    decimal CalculateDiscount(decimal totalBill);
}
```

এখন, রেগুলার কাস্টমারদের জন্য একটি ক্লাস বানানো হলো:
```csharp
public class RegularDiscount : IDiscountCalculator
{
    public decimal CalculateDiscount(decimal totalBill)
    {
        // ২০% ডিসকাউন্ট
        return totalBill * 0.20m; 
    }
}
```

কিন্তু আরেকজন জুনিয়র ডেভেলপার নতুন কাস্টমারদের জন্য একটি ডিসকাউন্ট ক্লাস বানাল। সে ভাবল, *"ডিসকাউন্ট মানেই তো মাইনাস, তাই আমি মাইনাস ভ্যালু রিটার্ন করি!"*
```csharp
public class NewCustomerDiscount : IDiscountCalculator
{
    public decimal CalculateDiscount(decimal totalBill)
    {
        // ❌ LSP Violation: মাইনাস (-) রিটার্ন করছে!
        // Parent কথা দিয়েছিল সে ডিসকাউন্ট অ্যামাউন্ট দেবে, 
        // কিন্তু এ দিয়ে দিচ্ছে নেগেটিভ ভ্যালু।
        return -50m; 
    }
}
```

### 💥 সমস্যাটা কোথায় হলো?

এখন আপনার `BillingService` ক্লাসে যখন বিল হিসাব করা হবে:
```csharp
public class BillingService
{
    public void PrintFinalBill(IDiscountCalculator discountCalc, decimal totalBill)
    {
        // বিল থেকে ডিসকাউন্ট মাইনাস করা হচ্ছে
        decimal finalBill = totalBill - discountCalc.CalculateDiscount(totalBill);
        Console.WriteLine($"Your final bill is: {finalBill}");
    }
}
```

**যদি RegularDiscount পাস করা হয়:**
```csharp
BillingService billing = new BillingService();
billing.PrintFinalBill(new RegularDiscount(), 1000); 
// 1000 - 200 = 800 টাকা। (একদম ঠিক কাজ করেছে!)
```

**কিন্তু যদি NewCustomerDiscount পাস করা হয়:**
```csharp
billing.PrintFinalBill(new NewCustomerDiscount(), 1000); 
// 1000 - (-50) = 1050 টাকা!
```

**কী মারাত্মক ভুল হলো খেয়াল করেছেন?** 
নতুন কাস্টমার ডিসকাউন্ট পাওয়ার কথা, উল্টো তার বিল ৫০ টাকা **বেড়ে গেল**! 
এখানে কিন্তু কোনো `Exception` থ্রো হয়নি, সিস্টেম ক্র্যাশও করেনি। কোড খুব সুন্দরভাবে রান হয়েছে। কিন্তু **আউটপুট বা রেজাল্ট পুরোপুরি ভুল এসেছে!** 

Parent (`IDiscountCalculator`) চুক্তি করেছিল সে ডিসকাউন্ট দেবে। কিন্তু Child (`NewCustomerDiscount`) এসে সেই চুক্তি ভেঙে নেগেটিভ ভ্যালু রিটার্ন করে পুরো ব্যবসার হিসাব পাল্টে দিয়েছে।

একেই বলে **"Major Behavior Change"**। Child ক্লাস কখনোই Parent ক্লাসের চুক্তির (Contract) বাইরের কোনো আচরণ করতে পারবে না। যদি করে, আর তার কারণে যদি সিস্টেমে লজিক্যাল ভুল হয়, তবে সেটিও মারাত্মক **LSP Violation**!

---

## 🏦 উদাহরণ ২: সাইলেন্ট চার্জ বা লুকানো ফি (Hidden Fees)

আর্থিক লেনদেনের আরও একটি খুব কমন রিয়েল-ওয়ার্ল্ড উদাহরণ দেখা যাক। 

ধরুন, আপনার একটি ব্যাংক অ্যাকাউন্ট ম্যানেজমেন্ট সিস্টেম আছে। সেখানে একটি বেস ইন্টারফেস বা চুক্তি আছে `IBankAccount`:
```csharp
public interface IBankAccount
{
    // Parent এর চুক্তি (Contract): "আমি ব্যালেন্স থেকে ঠিক ততটুকুই কাটব, যতটুকু উইথড্র (Withdraw) করা হবে।"
    // Postcondition: New Balance = Old Balance - amount
    void Withdraw(decimal amount);
}
```

সাধারণ সেভিংস অ্যাকাউন্টের ক্ষেত্রে এটি একদম ঠিকঠাক কাজ করে:
```csharp
public class SavingsAccount : IBankAccount
{
    public decimal Balance { get; set; } = 5000;

    public void Withdraw(decimal amount)
    {
        Balance -= amount; // চুক্তি অনুযায়ী ঠিক ততটুকুই কাটল
    }
}
```

কিন্তু ব্যাংকের একজন ডেভেলপার **"Credit Card Account"** নামে একটি নতুন ক্লাস বানাল এবং সেটিকে `IBankAccount` এর Child হিসেবে ডিক্লেয়ার করল। কিন্তু ক্রেডিট কার্ডের নিয়ম অনুযায়ী, টাকা তুললে (Cash Advance) হিডেন চার্জ বা ফি কাটে!
```csharp
public class CreditCardAccount : IBankAccount
{
    public decimal Balance { get; set; } = 5000;

    public void Withdraw(decimal amount)
    {
        // ❌ LSP Violation: Child চুপিচুপি চুক্তির বাইরে গিয়ে এক্সট্রা ফি কাটছে!
        decimal hiddenFee = 50; 
        Balance -= (amount + hiddenFee); 
    }
}
```

### 💥 সমস্যা কোথায়?

আপনার একটি `AtmMachine` ক্লাস আছে, যে শুধু Parent (`IBankAccount`) কে চেনে এবং ইউজারকে স্ক্রিনে মেসেজ দেখায়।
```csharp
public class AtmMachine
{
    public void ProcessWithdrawal(IBankAccount account, decimal amount)
    {
        account.Withdraw(amount);
        Console.WriteLine($"Successful! {amount} taka has been deducted from your account.");
    }
}
```

**কী মারাত্মক ভুল হলো?**
ইউজার এটিএম থেকে `1000` টাকা তুলল। `AtmMachine` স্ক্রিনে দেখাল: *"Successful! 1000 taka has been deducted from your account."*

কিন্তু যেহেতু এটি একটি `CreditCardAccount` (Child) ছিল, সে ব্যাকগ্রাউন্ডে ইউজারের অ্যাকাউন্ট থেকে `1000 + 50 = 1050` টাকা কেটে নিয়েছে! ইউজার জানতেই পারল না যে তার ৫০ টাকা বেশি কাটা গেছে, কারণ `AtmMachine` (যে Parent-এর চুক্তির ওপর বিশ্বাস করেছিল) জানতই না যে এই Child এক্সট্রা টাকা কাটতে পারে।

এখানেও **কোনো Exception থ্রো হয়নি, কোড ক্র্যাশ করেনি।** কিন্তু Parent (`IBankAccount`) যে কথা দিয়েছিল (যতটুকু তুলবে ততটুকুই কাটবে), Child (`CreditCardAccount`) এসে সেই চুক্তির বাইরে কাজ করে ইউজারের ব্যালেন্সের হিসাব পাল্টে দিয়েছে। 

**মূল কথা:** Parent-এর যে কাজ করার কথা (Behavior), Child-কে হুবহু সেই আচরণই ধরে রাখতে হবে। Child কখনোই Parent-এর চুক্তির বাইরে গিয়ে কোনো হিডেন বা সারপ্রাইজ কাজ করতে পারবে না। এটাই হলো LSP-এর "Behavioral" বা আচরণগত নিয়ম!

---

## 🎯 এক নজরে পুরো LSP (The Ultimate Summary)

LSP-এর একদম কোর (Core) বা মূল থিমটাকে আমরা ৩টি লাইনে সামারি করতে পারি:

1. **Parent should be replaceable by Child:** প্যারেন্টের জায়গায় চাইল্ড বসালে কোডে কোনো এরর হবে না।
2. **Child can't throw unexpected exceptions:** চাইল্ড এমন কোনো Exception থ্রো করতে পারবে না যা প্যারেন্টের চুক্তিতে (Contract) নেই।
3. **Child can't change the expected behavior:** চাইল্ড চুপিচুপি রুলস ভেঙে রেজাল্ট বা আচরণ পাল্টে দিতে পারবে না।

বাস্তব দুনিয়ার সফটওয়্যার ডেভেলপমেন্টে এই ৩টি কথাই হলো LSP-এর **৯৯%**! এইটুকু বুঝলে আপনি যেকোনো প্রজেক্টে অনায়াসে LSP মেইনটেইন করতে পারবেন।
