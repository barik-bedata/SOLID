using System;

namespace SOLID.ISP.Violation
{
    // ❌ ISP Violation: একটি বড় (Fat) ইন্টারফেসে সব মেথড রাখা হয়েছে।
    public interface IMultiFunctionPrinter
    {
        void Print();
        void Scan();
        void Fax();
    }

    // আধুনিক প্রিন্টার সব করতে পারে, তাই এর কোনো সমস্যা নেই।
    public class AdvancedPrinter : IMultiFunctionPrinter
    {
        public void Print() => Console.WriteLine("Printing document...");
        public void Scan() => Console.WriteLine("Scanning document...");
        public void Fax() => Console.WriteLine("Faxing document...");
    }

    // কিন্তু পুরনো প্রিন্টার শুধু প্রিন্ট করতে পারে। 
    // তবুও তাকে Scan এবং Fax ইমপ্লিমেন্ট করতে বাধ্য করা হচ্ছে!
    public class OldBasicPrinter : IMultiFunctionPrinter
    {
        public void Print() => Console.WriteLine("Printing document...");

        // ❌ ক্লায়েন্টকে এমন মেথড ইমপ্লিমেন্ট করতে বাধ্য করা হচ্ছে যা সে ব্যবহার করে না!
        public void Scan() => throw new NotSupportedException("Old printer cannot scan!");
        
        public void Fax() => throw new NotSupportedException("Old printer cannot fax!");
    }
}
