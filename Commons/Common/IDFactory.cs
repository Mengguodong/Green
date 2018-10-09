using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    /// <summary>
    /// 生成唯一id
    /// 马谦
    /// 2015年6月3日15:10:14
    /// </summary>
    public class IDFactory
    {
        private static IDFactory factory;
        /// <summary>
        /// 调用此方法获取此流水号不重复部分（7位）
        /// </summary>
        public static string GetNum(int type)
        {

            if (factory == null)
            {
                factory = new IDFactory();
            }
            string str = factory.Next().ToString();
            return type + str.Substring(3, str.Length - 3);
        }


        private IDFactory()
        {
            InitMillisecond = (ulong)(DateTime.Now - DateTime.Parse("2000-1-1")).TotalMilliseconds;
            mWatch.Restart();
        }

        private ulong mCurrentMillisecond = 0;

        private byte mSeed;

        private System.Diagnostics.Stopwatch mWatch = new System.Diagnostics.Stopwatch();

        public void LoadIPGroup()
        {
            string hostName = Dns.GetHostName();

            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Group = (byte)(ip.Address >> 24);
                    break;
                }

            }
        }

        public byte Group
        {
            get;
            set;
        }

        private ulong InitMillisecond;

        public ulong Next()
        {
            ulong result = 0;
            System.Threading.SpinLock slock = new System.Threading.SpinLock();
            bool gotLock = false;
            try
            {
                while (!gotLock)
                {
                    slock.Enter(ref gotLock);
                    if (gotLock)
                    {
                        ulong cms = (ulong)mWatch.Elapsed.TotalMilliseconds - InitMillisecond;
                        if (cms != mCurrentMillisecond)
                        {
                            mSeed = 0;
                            mCurrentMillisecond = cms;
                        }
                        result = ((ulong)Group << 48) | (mCurrentMillisecond << 8) | (ulong)mSeed;
                        mSeed++;

                    }
                }
            }
            finally
            {

                if (gotLock) slock.Exit();
            }
            return result;

        }
    }
}
