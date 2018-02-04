using System;
using System.Text;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace FileDriverLib
{
    /// <summary>
    /// 内存映射文件，托管代码
    /// 内存与文件直接关联，持久化操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MemoryDriver<T> where T:struct
    {
        MemoryMappedFile mmf = null;

        MemoryMappedViewAccessor mmva = null;

        int perBlockSize = 10 * 1024 * 1024; //映射内存大小 10M

        public MemoryDriver()
        {
            mmf = MemoryMappedFile.CreateFromFile(Path.Combine(AppContext.BaseDirectory, "colors.txt"), FileMode.OpenOrCreate, "ShareFile", perBlockSize,MemoryMappedFileAccess.ReadWrite);
            mmva = mmf.CreateViewAccessor();
        }   

        public void WriteMemory(T obj)
        {
            ///位置0 写入结构体T
            mmva.Write<T>(0, ref obj);
        }

        public T ReadMemory()
        {
            T obj;
            mmva.Read<T>(0, out obj);
            return obj;
        }

        public void Dispose()
        {
            if (mmva != null)
            {
                mmva.Dispose();
            }
            if (mmf != null)
                mmf.Dispose();
        }
    }
}
