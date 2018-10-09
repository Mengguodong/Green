using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 数据类型转换帮助类
    /// Author:徐江安
    /// 更细时间：2015年11月16日 17:18:41
    /// </summary>
    public static class ConvertHelper
    {
        /// <summary>
        ///     转换为Int32 
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static Int32 ToInt32(object value, Int32 defValue = 0)
        {
            var val = Convert.ToString(value);
            Int32 rv;
            return Int32.TryParse(val, out rv) ? rv : defValue;
        }

        // <summary>
        /// 转换为Double 
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static Double ToDouble(object value, Double defValue = 0)
        {
            var val = Convert.ToString(value);
            Double rv;
            return Double.TryParse(val, out rv) ? rv : defValue;
        }

        /// <summary>
        ///     转换为Decimal 
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static Decimal ToDecimal(object value, Decimal defValue = 0)
        {
            var val = Convert.ToString(value);
            Decimal rv;
            return Decimal.TryParse(val, out rv) ? rv : defValue;
        }

        /// <summary>
        ///     转换DateTime
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defValue">请自定义转换失败的值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value, DateTime defValue)
        {
            var val = Convert.ToString(value);
            DateTime rv;
            return DateTime.TryParse(val, out rv) ? rv : defValue;
        }

        /// <summary>
        /// 转换金额，四舍五入
        /// author:xiaoy
        /// 2015-09-29 16:10:25 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Round(decimal v, int x)
        {
            bool isNegative = false;
            //如果是负数
            if (v < 0)
            {
                isNegative = true;
                v = -v;
            }

            int IValue = 1;
            for (int i = 1; i <= x; i++)
            {
                IValue = IValue * 10;
            }
            decimal Int = Math.Round(v * IValue + 0.5M, 0);
            v = Int / IValue;

            if (isNegative)
            {
                v = -v;
            }

            return v;
        }

        /// <summary>
        /// 将合并后的字符串集合，转换成int类型集合
        /// 创建人：王刚
        /// 创建时间：2016年4月22日
        /// </summary>
        /// <param name="joinedItemColl">间隔符合并后的字符串，如1,2,3,4</param>
        /// <returns></returns>
        public static List<int> ToIntColl(string joinedItemColl)
        {
            List<int> res = new List<int>();
            if (!string.IsNullOrWhiteSpace(joinedItemColl))
            {
                var itemColl = joinedItemColl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                bool emptyColl = itemColl == null || itemColl.Length == 0;
                if (!emptyColl)
                {
                    int intItem = 0;
                    foreach (var item in itemColl)
                    {
                        if (int.TryParse(item, out intItem))
                        {
                            res.Add(intItem);
                        }
                    }
                }
            }
            return res;
        }
    }
}