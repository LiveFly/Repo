using System;
using System.Collections.Generic;
using System.Text;

namespace FileDriverTest
{
    public struct MyColor
    {
        public short Red;
        public short Green;
        public short Blue;
        public short Alpha;

        public void Brighten(short value)
        {
            Red = (short)Math.Min(short.MaxValue, (int)Red + value);
            Green = (short)Math.Min(short.MaxValue, (int)Green + value);
            Blue = (short)Math.Min(short.MaxValue, (int)Blue + value);
            Alpha = (short)Math.Min(short.MaxValue, (int)Alpha + value);
        }
    }
}
