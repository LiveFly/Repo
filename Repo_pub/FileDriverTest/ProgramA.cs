using System;
using FileDriverLib;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading.Tasks;

namespace FileDriverTest
{
    /// <summary>
    /// Process A
    /// </summary>
    class ProgramA
    {
        static void Main(string[] args)
        {
            MemoryTasksDriver md = new MemoryTasksDriver();
            md.Write();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }

 
}
