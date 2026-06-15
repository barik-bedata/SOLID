# Single Responsibility Principle (SRP)

A class should have one, and only one, reason to change.

In other words, every module, class, or function in a program should have responsibility over a single part of that program's functionality, and it should encapsulate that functionality.

---

## 🛑 The Violation (English)

Take a look at the code in [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/01-single-responsibility-principle/Violation.cs).

In this example, the class `OrderProcessor` has **three reasons to change**:
1. **Tax Calculation & Pricing Logic**: If the tax calculation formula changes (e.g., adding dynamic state taxes or discounts), we have to modify `OrderProcessor`.
2. **Persistence Logic**: Currently, the order is saved as a JSON file using `System.IO`. If we decide to use a database (e.g., SQL Server or MongoDB), we have to modify `OrderProcessor`.
3. **Notification Logic**: Currently, it sends confirmation emails using a direct `SmtpClient`. If we switch to a transactional email API provider (like SendGrid or AWS SES) or want to change the email template, we have to modify `OrderProcessor`.

---

## 🟢 The Solution (English)

Take a look at the code in [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/01-single-responsibility-principle/Solution.cs).

We refactored the code by breaking down the concerns:
1. **`ITaxCalculator`**: Handles business rules for calculating order tax.
2. **`IOrderRepository`**: Handles how the order is persisted (JSON file, database, memory, etc.).
3. **`INotificationService`**: Handles how notifications are sent (SMTP, SendGrid, SMS, etc.).
4. **`OrderProcessor`**: Now acts purely as an orchestrator. It orchestrates the process of completing an order without knowing *how* the tax is computed, *where* the order is saved, or *how* the customer is notified.

Now, `OrderProcessor` has **only one reason to change**: if the high-level workflow of processing an order changes (e.g., if we decide that we should only send emails *after* a payment is verified).

---

## 🇧🇩 সহজ বাংলায় ব্যাখ্যা (Single Responsibility Principle)

সহজ বাস্তব জীবনের উদাহরণ দিয়ে নিচে বুঝিয়ে দেওয়া হলো:

### ১. বাস্তব জীবনের উদাহরণ (Real-life Analogy)

মনে করুন, আপনার একটা রেস্টুরেন্ট আছে। সেখানে আপনি একজন মাত্র কর্মচারী রেখেছেন—তার নাম **করিম**। 

করিমের কাজ হলো:
1. কাস্টমারের কাছ থেকে **অর্ডার নেওয়া**।
2. কিচেনে গিয়ে **রান্না করা**।
3. কাস্টমারের টেবিল **পরিষ্কার করা**।
4. দিন শেষে দোকানের **হিসাব-নিকাশ মেলানো**।

**সমস্যা:**
যদি অনেক কাস্টমার একসঙ্গে চলে আসে, করিম একা সবকিছু সামলাতে গিয়ে হিমশিম খাবে। রান্নার রেসিপি পরিবর্তন হলে করিমকে নতুন করে রান্না শিখতে হবে, তখন টেবিল পরিষ্কার বা হিসাব বন্ধ হয়ে যাবে। যেকোনো একটি কাজের পরিবর্তনের জন্য করিমের ওপর চাপ পড়বে।

**সমাধান (SRP):**
আপনি করিমের লোড কমিয়ে চারজন আলাদা লোক নিয়োগ করলেন:
1. **শেফ (Chef):** কাজ শুধু রান্না করা।
2. **ওয়েটার (Waiter):** কাজ অর্ডার নেওয়া ও খাবার পরিবেশন করা।
3. **ক্লিনার (Cleaner):** কাজ টেবিল পরিষ্কার করা।
4. **ক্যাশিয়ার (Cashier):** কাজ হিসাব-নিকাশ করা।

এখানে প্রত্যেকে **একটি মাত্র কাজের জন্য দায়ী (Single Responsibility)**। যদি রান্নার রেসিপি পরিবর্তন হয়, ক্যাশিয়ারের এতে কিছুই যায় আসে না। তার কাজ সে করতেই থাকবে।

### ২. কোডের সাথে মিল

[Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/01-single-responsibility-principle/Violation.cs)-এ `OrderProcessor` ক্লাসটি হলো ওই একা কাজ করা **করিম**-এর মতো। সে একাই ট্যাক্স হিসাব করে, ফাইলে ডাটা সেভ করে এবং ইমেইল পাঠায়। এর ফলে যেকোনো একটি পরিবর্তন পুরো ফাইলটিকে বারবার এডিট করতে বাধ্য করে।

[Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/01-single-responsibility-principle/Solution.cs)-এ আমরা কাজের দায়িত্বগুলো ভাগ করে দিয়েছি:
1. ট্যাক্স হিসাবের দায়িত্ব দিয়েছি `StandardTaxCalculator` কে।
2. ডাটা সেভ করার দায়িত্ব দিয়েছি `JsonFileOrderRepository` কে।
3. ইমেইল পাঠানোর দায়িত্ব দিয়েছি `SmtpNotificationService` কে।

`OrderProcessor` এখন শুধু একজন ম্যানেজারের মতো কাজগুলো করিয়ে নেয়। ফলে ভবিষ্যৎ পরিবর্তন অনেক সহজ ও বাগ-মুক্ত (Bug-free) হয়।

---

## 🔗 SRP এবং Cohesion-এর সম্পর্ক (Low vs High Cohesion)

সফটওয়্যার ইঞ্জিনিয়ারিংয়ের একটি অত্যন্ত জনপ্রিয় এবং গোল্ডেন রুল (Golden Rule) আছে:
> **"High Cohesion, Loose Coupling"** (কোডকে সবসময় High Cohesion এবং Loose Coupling হতে হবে)। 

Loose Coupling আমরা অর্জন করি **DIP (Dependency Inversion Principle)** দিয়ে। আর **High Cohesion** অর্জন করি এই **SRP (Single Responsibility Principle)** দিয়ে! 

### ১. Low Cohesion (এলোমেলো জগাখিচুড়ি)
Cohesion মানে হলো "মিল" বা "একতা"। একটি ক্লাসের ভেতরের মেথডগুলোর মধ্যে যদি কোনো মিল না থাকে, তখন তাকে বলে **Low Cohesion**। 
যেমন আমাদের `Violation.cs`-এর `OrderProcessor` ক্লাসটি ট্যাক্স হিসাব করে, ফাইলে সেভ করে এবং ইমেইল পাঠায়। এই তিনটি কাজের মধ্যে কোনো মিল বা একতা নেই। ট্যাক্স হিসাবের সাথে ইমেইল পাঠানোর কোনো সম্পর্ক নেই। 
অর্থাৎ এখানে Cohesion খুবই **Low**। আর এটিই হলো **SRP Violation**!

### ২. High Cohesion (নিখুঁত একতা)
SRP বলে, একটি ক্লাসের একটাই দায়িত্ব থাকবে। 
যখন আপনি জগাখিচুড়ি ক্লাসটি ভেঙে আলাদা ক্লাস বানালেন (যেমন `SmtpNotificationService`), তখন ওই ক্লাসের ভেতরে যা যা মেথড বা প্রপার্টি থাকবে, তার সবকিছুই শুধুমাত্র "নোটিফিকেশন বা ইমেইল" রিলেটেড হবে। অর্থাৎ মেথডগুলোর মধ্যে স্ট্রং মিল বা একতা আছে। একেই বলে **High Cohesion**! 

**সারসংক্ষেপ:**
* **SRP মানলে:** কোডের **Cohesion বাড়ে (High Cohesion)**। ক্লাসগুলো ফোকাসড এবং গোছানো হয়।
* **DIP মানলে:** কোডের **Coupling কমে (Loose Coupling)**। ক্লাসগুলো স্বাধীন হয়।
