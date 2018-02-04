using System;
using System.Threading;
using System.Diagnostics;
using Common;

namespace CpuCore
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Program Cpu Core");
            Console.WriteLine("Cpu Core Number:" + Environment.ProcessorCount);

            /// 方法1
            //int numOfCPUs = Environment.ProcessorCount;
            //for (int i = 0; i < numOfCPUs; ++i)
            //{
            //    ThreadPool.QueueUserWorkItem(SinWave);
            //}
            //Thread.Sleep(Timeout.Infinite);

            ///方法2
            SinWave2();
        }

        private static void SinWave2(object dummy = null)
        {
            //CAPI.SetProcessAffinityMask(CAPI.GetCurrentThread(), new UIntPtr(0x00000001));
            ///0x08 第4核 4 第三核 2第二核 1第1核
            CAPI.SetThreadAffinityMask(CAPI.GetCurrentThread(), new IntPtr(0x08));
            while (true)
            {
                for (double i = 0.0; i < 2 * Math.PI; i += 0.1)
                {
                    Compute(500, Math.Sin(i) / 2.0 + 0.5);
                }
            }
        }

        private static void SinWave(object dummy)
        {
            while (true)
            {
                for (double i = 0.0; i < 2 * Math.PI; i += 0.1)
                {
                    Compute(500, Math.Sin(i) / 2.0 + 0.5);
                }
            }
        }

        private static Stopwatch m_sw = new Stopwatch();
        private static void Compute(long time, double percent)
        {
            long runTime = (long)(time * percent);
            long sleepTime = time - runTime;
            m_sw.Start();
            while (m_sw.ElapsedMilliseconds - runTime < 0)
            {
                
            }
            m_sw.Stop();
            m_sw.Reset();
            Thread.Sleep((int)sleepTime);
        }
    }
}