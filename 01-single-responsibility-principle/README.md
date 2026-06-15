# Single Responsibility Principle (SRP)

A class should have one, and only one, reason to change.

In other words, every module, class, or function in a program should have responsibility over a single part of that program's functionality, and it should encapsulate that functionality.

---

## 🛑 The Violation

Take a look at the code in [Violation.cs](file:///Users/bedata/Desktop/Learning/SOLID/01-single-responsibility-principle/Violation.cs).

In this example, the class `OrderProcessor` has **three reasons to change**:
1. **Tax Calculation & Pricing Logic**: If the tax calculation formula change (e.g., adding dynamic state taxes or discounts), we have to modify `OrderProcessor`.
2. **Persistence Logic**: Currently, the order is saved as a JSON file using `System.IO`. If we decide to use a PostgreSQL database with Entity Framework Core, we have to modify `OrderProcessor`.
3. **Notification Logic**: Currently, it sends confirmation emails using a direct `SmtpClient`. If we switch to a transactional email API provider (like SendGrid or AWS SES) or want to change the email body template, we have to modify `OrderProcessor`.

---

## 🟢 The Solution

Take a look at the code in [Solution.cs](file:///Users/bedata/Desktop/Learning/SOLID/01-single-responsibility-principle/Solution.cs).

We refactored the code by breaking down the concerns:
1. **`ITaxCalculator`**: Handles business rules for calculating order tax.
2. **`IOrderRepository`**: Handles how the order is persisted (JSON file, database, memory, etc.).
3. **`INotificationService`**: Handles how notifications are sent (SMTP, SendGrid, SMS, etc.).
4. **`OrderProcessor`**: Now acts purely as an orchestrator. It orchestrates the process of completing an order without knowing *how* the tax is computed, *where* the order is saved, or *how* the customer is notified.

Now, `OrderProcessor` has **only one reason to change**: if the high-level workflow of processing an order changes (e.g., if we decide that we should only send emails *after* a payment is verified).

---

## 💡 Real-World Benefits

* **Easier Testing**: You can now unit test `OrderProcessor` by mocking `IOrderRepository` and `INotificationService` without writing files to disk or sending real emails.
* **Maintainability**: If the database schema changes, you only modify `JsonFileOrderRepository` or swap it out. The payment flow and email sending logic remain completely untouched and safe from accidental regressions.
* **High Reusability**: You can reuse `SmtpNotificationService` for other flows (like password resets or shipping updates) without copying code.
