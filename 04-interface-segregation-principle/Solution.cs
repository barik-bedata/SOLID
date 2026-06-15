using System;

namespace SOLID.ISP.Solution
{
    // ✅ ISP Solution: বড় ইন্টারফেসকে ভেঙে ছোট ও নির্দিষ্ট (Specific) ইন্টারফেস বানানো হয়েছে।
    
    public interface IPrinter
    {
        void Print();
    }

    public interface IScanner
    {
        void Scan();
    }

    public interface IFax
    {
        void Fax();
    }

    // আধুনিক প্রিন্টার যা যা পারে, শুধু সেই ইন্টারফেসগুলোই ইমপ্লিমেন্ট করবে।
    public class AdvancedPrinter : IPrinter, IScanner, IFax
    {
        public void Print() => Console.WriteLine("Printing document...");
        public void Scan() => Console.WriteLine("Scanning document...");
        public void Fax() => Console.WriteLine("Faxing document...");
    }

    // পুরনো প্রিন্টার শুধু প্রিন্ট করতে পারে, তাই সে শুধুমাত্র IPrinter ইমপ্লিমেন্ট করবে।
    // তাকে অকারণে Scan বা Fax ইমপ্লিমেন্ট করতে বাধ্য করা হচ্ছে না।
    public class OldBasicPrinter : IPrinter
    {
        public void Print() => Console.WriteLine("Printing document...");
    }
}
