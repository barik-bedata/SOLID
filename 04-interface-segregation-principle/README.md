# Interface Segregation Principle (ISP)

Clients should not be forced to depend upon interfaces that they do not use.

সহজ কথায়: **কোনো ক্লাসকে এমন কোনো ইন্টারফেস ইমপ্লিমেন্ট করতে বাধ্য করা উচিত নয়, যার সব মেথড তার দরকার নেই।** একটি বড়, মোটা (Fat) ইন্টারফেস বানানোর চেয়ে, কাজের ধরন অনুযায়ী ছোট ছোট স্পেসিফিক (Specific) ইন্টারফেস বানানো অনেক ভালো।

---

## 🛑 The Violation — [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/04-interface-segregation-principle/Violation.cs)

ধরুন আমাদের অফিসে বিভিন্ন ধরনের প্রিন্টার আছে। আমরা একটি ইন্টারফেস বানালাম `IMultiFunctionPrinter`, যার ভেতরে প্রিন্ট, স্ক্যান এবং ফ্যাক্স করার মেথড আছে।

```csharp
public interface IMultiFunctionPrinter
{
    void Print();
    void Scan();
    void Fax();
}
```

আধুনিক একটি প্রিন্টার (`AdvancedPrinter`) এই সবগুলো কাজ করতে পারে। তাই তার জন্য এই ইন্টারফেস ইমপ্লিমেন্ট করা কোনো সমস্যার কিছু নয়।

কিন্তু সমস্যা হলো একটি পুরনো প্রিন্টারের (`OldBasicPrinter`) ক্ষেত্রে, যেটি শুধু প্রিন্ট করতে পারে। কিন্তু যেহেতু সে `IMultiFunctionPrinter` ইমপ্লিমেন্ট করেছে, তাকে বাধ্য হয়ে `Scan()` এবং `Fax()` মেথডও লিখতে হচ্ছে!

```csharp
public class OldBasicPrinter : IMultiFunctionPrinter
{
    public void Print() => Console.WriteLine("Printing...");

    // ❌ ISP Violation: অকারণে এই মেথডগুলো লিখতে বাধ্য করা হচ্ছে!
    public void Scan() => throw new NotSupportedException("Old printer cannot scan!");
    public void Fax() => throw new NotSupportedException("Old printer cannot fax!");
}
```

**সমস্যা কোথায় (ISP Violation)?** 
`OldBasicPrinter`-এর স্ক্যান বা ফ্যাক্স করার ক্ষমতাই নেই। তবুও তাকে এমন একটি ইন্টারফেসের ওপর নির্ভর করতে বাধ্য করা হয়েছে যা তার কোনো কাজেই লাগে না। এর ফলে কোডে অকারণে `NotSupportedException` লিখতে হচ্ছে এবং কোড নোংরা (Polluted) হচ্ছে।

---

## 🟢 The Solution — [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/04-interface-segregation-principle/Solution.cs)

বড় ইন্টারফেসটিকে ভেঙে ছোট ছোট কাজের ভিত্তিতে ভাগ (Segregate) করে ফেলতে হবে।

```csharp
public interface IPrinter { void Print(); }
public interface IScanner { void Scan(); }
public interface IFax { void Fax(); }
```

এখন যে মেশিনের যা যা ক্ষমতা আছে, সে শুধু সেই ইন্টারফেসগুলোই ব্যবহার করবে:

```csharp
// আধুনিক প্রিন্টার ৩টি ইন্টারফেসই ব্যবহার করবে
public class AdvancedPrinter : IPrinter, IScanner, IFax
{
    public void Print() => Console.WriteLine("Printing...");
    public void Scan() => Console.WriteLine("Scanning...");
    public void Fax() => Console.WriteLine("Faxing...");
}

// পুরনো প্রিন্টার এখন শুধু IPrinter ব্যবহার করবে!
public class OldBasicPrinter : IPrinter
{
    public void Print() => Console.WriteLine("Printing...");
}
```

**লাভ কী হলো?** 
`OldBasicPrinter`-কে আর জোর করে এমন কোনো মেথড ইমপ্লিমেন্ট করতে হলো না যা সে ব্যবহার করবে না। ক্লাসগুলো ক্লিন রইল এবং "বড় ও অপ্রয়োজনীয় ইন্টারফেস"-এর হাত থেকে মুক্তি মিলল। এটিই হলো **Interface Segregation Principle**!
