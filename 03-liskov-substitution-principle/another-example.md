# Liskov Substitution Principle (LSP) — C# Example

## LSP কী?

> **"Child class কে যদি Parent class এর জায়গায় বসাও, তাহলে কোনো সমস্যা হওয়া উচিত না।"**

সহজ কথায় — তোমার code যদি `Parent` বা `Interface` দিয়ে কাজ করে, তাহলে সেখানে যেকোনো `Child` বসালে সব আগের মতোই চলবে। Error আসবে না, ভুল result আসবে না।

---

## Project Structure

```
LSP Example/
│
├── Interfaces/
│   ├── IPayment.cs       ← Payment এর Parent (Interface)
│   └── IOrder.cs         ← Order এর Parent (Interface)
│
├── Payments/
│   ├── Bkash.cs          ← IPayment এর Child
│   ├── Nagad.cs          ← IPayment এর Child
│   └── Rocket.cs         ← IPayment এর Child
│
├── Orders/
│   ├── RegularOrder.cs   ← IOrder এর Child
│   ├── BulkOrder.cs      ← IOrder এর Child
│   └── SubscriptionOrder.cs ← IOrder এর Child
│
└── Program.cs            ← Main entry point
```

---

## Interfaces (Parent)

### IPayment

```csharp
public interface IPayment
{
    void Pay(int amount);
    string GetPaymentMethod();
}
```

### IOrder

```csharp
public interface IOrder
{
    void Checkout(IPayment payment);
    string GetOrderSummary();
}
```

এই দুইটা Interface হলো **Parent**। এরা শুধু বলে "কী কী কাজ করতে হবে" — কিন্তু কীভাবে করতে হবে সেটা বলে না।

---

## Payment Classes (Child of IPayment)

### Bkash

```csharp
public class Bkash : IPayment
{
    private string _phoneNumber;

    public Bkash(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public void Pay(int amount)
    {
        Console.WriteLine($"Bkash থেকে {_phoneNumber} নম্বরে {amount} টাকা পাঠানো হলো ✅");
    }

    public string GetPaymentMethod() => "Bkash";
}
```

### Nagad

```csharp
public class Nagad : IPayment
{
    private string _phoneNumber;

    public Nagad(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public void Pay(int amount)
    {
        Console.WriteLine($"Nagad থেকে {_phoneNumber} নম্বরে {amount} টাকা পাঠানো হলো ✅");
    }

    public string GetPaymentMethod() => "Nagad";
}
```

### Rocket

```csharp
public class Rocket : IPayment
{
    private string _phoneNumber;

    public Rocket(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public void Pay(int amount)
    {
        Console.WriteLine($"Rocket থেকে {_phoneNumber} নম্বরে {amount} টাকা পাঠানো হলো ✅");
    }

    public string GetPaymentMethod() => "Rocket";
}
```

---

## Order Classes (Child of IOrder)

### RegularOrder — সাধারণ Order

```csharp
public class RegularOrder : IOrder
{
    private string _productName;
    private int _price;

    public RegularOrder(string productName, int price)
    {
        _productName = productName;
        _price = price;
    }

    public void Checkout(IPayment payment)
    {
        Console.WriteLine("\n----------------------------");
        Console.WriteLine(GetOrderSummary());
        Console.WriteLine($"Payment  : {payment.GetPaymentMethod()}");
        payment.Pay(_price);
        Console.WriteLine("Order Confirmed! 🎉");
        Console.WriteLine("----------------------------");
    }

    public string GetOrderSummary()
    {
        return $"[Regular Order]\nProduct: {_productName} | Price: {_price} টাকা";
    }
}
```

### BulkOrder — বেশি পরিমাণ Order, 10% Discount আছে

```csharp
public class BulkOrder : IOrder
{
    private string _productName;
    private int _price;
    private int _quantity;

    public BulkOrder(string productName, int price, int quantity)
    {
        _productName = productName;
        _price = price;
        _quantity = quantity;
    }

    public void Checkout(IPayment payment)
    {
        int total = _price * _quantity;
        int discount = total / 10;
        int finalPrice = total - discount;

        Console.WriteLine("\n----------------------------");
        Console.WriteLine(GetOrderSummary());
        Console.WriteLine($"Discount : {discount} টাকা বাদ");
        Console.WriteLine($"Final    : {finalPrice} টাকা");
        Console.WriteLine($"Payment  : {payment.GetPaymentMethod()}");
        payment.Pay(finalPrice);
        Console.WriteLine("Bulk Order Confirmed! 🎉");
        Console.WriteLine("----------------------------");
    }

    public string GetOrderSummary()
    {
        return $"[Bulk Order]\nProduct: {_productName} | {_quantity}টি × {_price} টাকা";
    }
}
```

### SubscriptionOrder — মাসে মাসে payment কাটবে

```csharp
public class SubscriptionOrder : IOrder
{
    private string _planName;
    private int _monthlyPrice;

    public SubscriptionOrder(string planName, int monthlyPrice)
    {
        _planName = planName;
        _monthlyPrice = monthlyPrice;
    }

    public void Checkout(IPayment payment)
    {
        Console.WriteLine("\n----------------------------");
        Console.WriteLine(GetOrderSummary());
        Console.WriteLine($"Payment  : {payment.GetPaymentMethod()}");
        payment.Pay(_monthlyPrice);
        Console.WriteLine("Subscription চালু হয়ে গেছে! 🎉");
        Console.WriteLine("----------------------------");
    }

    public string GetOrderSummary()
    {
        return $"[Subscription]\nPlan: {_planName} | মাসিক: {_monthlyPrice} টাকা";
    }
}
```

---

## Program.cs — সব একসাথে চালাই

```csharp
public class Program
{
    // ⭐ LSP এখানে —
    // IOrder এর জায়গায় যেকোনো Child বসাও
    // IPayment এর জায়গায় যেকোনো Child বসাও
    // কোনো সমস্যা নাই!
    public static void ProcessOrder(IOrder order, IPayment payment)
    {
        order.Checkout(payment);
    }

    public static void Main()
    {
        // Payment methods
        IPayment bkash  = new Bkash("01711-111111");
        IPayment nagad  = new Nagad("01811-222222");
        IPayment rocket = new Rocket("01911-333333");

        // Orders
        IOrder regular      = new RegularOrder("Nike Shoes", 3500);
        IOrder bulk         = new BulkOrder("Water Bottle", 200, 50);
        IOrder subscription = new SubscriptionOrder("Netflix BD", 450);

        // IOrder + IPayment — দুইটাই Replace হচ্ছে ✅
        ProcessOrder(regular, bkash);
        ProcessOrder(bulk, nagad);
        ProcessOrder(subscription, rocket);
    }
}
```

---

## Output

```
----------------------------
[Regular Order]
Product: Nike Shoes | Price: 3500 টাকা
Payment  : Bkash
Bkash থেকে 01711-111111 নম্বরে 3500 টাকা পাঠানো হলো ✅
Order Confirmed! 🎉
----------------------------

----------------------------
[Bulk Order]
Product: Water Bottle | 50টি × 200 টাকা
Discount : 1000 টাকা বাদ
Final    : 9000 টাকা
Payment  : Nagad
Nagad থেকে 01811-222222 নম্বরে 9000 টাকা পাঠানো হলো ✅
Bulk Order Confirmed! 🎉
----------------------------

----------------------------
[Subscription]
Plan: Netflix BD | মাসিক: 450 টাকা
Payment  : Rocket
Rocket থেকে 01911-333333 নম্বরে 450 টাকা পাঠানো হলো ✅
Subscription চালু হয়ে গেছে! 🎉
----------------------------
```

---

## LSP কোথায় হলো? একনজরে দেখো

```csharp
// এই একটা function — Interface ছাড়া কিছুই চেনে না
public static void ProcessOrder(IOrder order, IPayment payment)
{
    order.Checkout(payment);
}

// কিন্তু যেকোনো combination দাও — সব কাজ করে ✅
ProcessOrder(new RegularOrder(...),      new Bkash(...));   // ✅
ProcessOrder(new BulkOrder(...),         new Nagad(...));   // ✅
ProcessOrder(new SubscriptionOrder(...), new Rocket(...));  // ✅

// কাল নতুন কিছু যোগ করলেও — ProcessOrder ছুঁতে হবে না! 🔥
ProcessOrder(new GiftOrder(...),         new CashOnDelivery(...)); // ✅
```

---

## মনে রাখার নিয়ম

| বিষয় | কথা |
|-------|------|
| Interface | Parent — শুধু নিয়ম বলে |
| Implementing Class | Child — নিয়ম মেনে কাজ করে |
| কে replace হয়? | Child, Parent এর জায়গায় বসে |
| LSP মানা হলে | Child বসালে সব আগের মতো কাজ করে |
| LSP ভাঙলে | Child বসালে Error বা ভুল result আসে |

> **সহজ test:** নিজেকে জিজ্ঞেস করো — "আমার Child class কি সব জায়গায় Parent এর মতো কাজ করতে পারবে?" হ্যাঁ হলে LSP ঠিক আছে ✅, না হলে design বদলাও ❌
