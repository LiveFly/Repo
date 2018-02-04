using System;
using System.Diagnostics;
using System.Threading;

namespace HighCpuAnalyzer_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Process.GetCurrentProcess().Id);
            new Thread(A).Start();
            new Thread(B).Start();
            Console.WriteLine("Demo Running");
            Console.ReadKey();
        }

        private static void A(object state)
        {
            while (true)
            {
                //Thread.Sleep(1000);
            }
        }

        private static void B(object state)
        {
            while (true)
            {
                //double d = new Random().NextDouble() * new Random().NextDouble();
            }
        }
    }
}
