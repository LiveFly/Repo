using System;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;
using System.Text;

namespace FileDriverLib
{
    /// <summary>
    /// 共享内存，跨进程间通信
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MemoryTasksDriver
    {
        int memorysize = 1 * 1024 * 1024; //1M

        string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MemoryShare.txt");

        public MemoryTasksDriver()
        {
            //Task.Run(new Action(Read));
        }

        /// <summary>
        /// Process A
        /// </summary>
        public void Write()
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fileName, FileMode.OpenOrCreate, "testmap", 10000))
            {
                bool mutexCreated;
                Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
                Console.WriteLine("writer processA");
                mutex.WaitOne();
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);
                    writer.Write(DateTime.UtcNow.ToString());
                    Console.WriteLine(DateTime.UtcNow.ToString());
                }
                Console.ReadKey();
                mutex.ReleaseMutex();

                mutex.WaitOne();
                Console.ReadKey();
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
                    Console.WriteLine("Process A: {0}", reader.ReadString());
                }
                mutex.ReleaseMutex();
            }
            Console.WriteLine("测试： 启动啦！！！");
        }

        /// <summary>
        /// Process B
        /// </summary>
        public void Read()
        {
            while (true)
            {
                try
                {
                    using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("testmap"))
                    {
                        Mutex mutex = Mutex.OpenExisting("testmapmutex");
                        Console.WriteLine("ProcessB waitone");
                        mutex.WaitOne();
                        using (MemoryMappedViewStream stream = mmf.CreateViewStream(0, 0))
                        {
                            BinaryReader bread = new BinaryReader(stream, Encoding.UTF8);
                            Console.WriteLine(bread.ReadString());
                        }
                        mutex.ReleaseMutex();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}
