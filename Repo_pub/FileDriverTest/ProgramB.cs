using FileDriverLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileDriverTest
{
    /// <summary>
    /// Process B
    /// </summary>
    class ProgramB
    {
        static void Main(string[] args)
        {
            MemoryTasksDriver md = new MemoryTasksDriver();
            md.Read();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
