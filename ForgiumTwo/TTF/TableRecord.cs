using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgiumTwo.TTF
{
    public struct TableRecord
    {
        public string Tag;
        public uint CheckSum;
        public uint Offset;
        public uint Length;
    }
}
