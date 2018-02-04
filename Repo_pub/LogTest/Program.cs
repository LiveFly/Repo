using System;
using System.Threading.Tasks;
using LogLib;

namespace LogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Init();
            Action func = DoTask;
            Task.Run(func);
            Task.Run(func);
            Task.Delay(10000);
            Task.Run(new Action(DoDebug));
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        static void DoTask()
        {
            int? id = Task.CurrentId;
           
            //for (int i = 0; i < 100; i++)
            //{
            //    Exception e = new Exception(id + ":" + i.ToString() + ":" + DateTime.Now);
            //    Logger.Warn(e, "warn");
            //    Logger.Warn(e, "error");
            //    Logger.Warn(e, "debug");
            //}

            try
            {
                int i = 5;
                i =i/ 0;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        static void DoDebug()
        {
            int? id = Task.CurrentId;
            for(int i = 0; i < 100; i++)
            {
                Logger.Debug(id + ":" + i.ToString() + ":" + DateTime.Now);
            }
        }
    }
}
