using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class SerialNumberHelper
    {
        private static Random _r;

        private static Random R
        {
            get
            {
                if (_r == null)
                {
                    return new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
                }
                return _r;
            }
        }
        public static string Factory()
        {
            return DateTime.Now.ToString("yyyyMMdd").Substring(2, 6) + R.Next().ToString("0000000000") + DateTime.Now.Millisecond.ToString("000");
        }
    }
}
