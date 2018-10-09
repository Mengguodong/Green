using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    /// <summary>
    /// desc:枚举辅助类
    /// date:2015年12月30日
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 返回枚举项的描述信息
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项</param>
        /// <returns>枚举想的描述信息</returns>
        public static string GetDescription(Enum value)
        {
            var enumType = value.GetType();
            // 获取枚举常数名称。
            var name = Enum.GetName(enumType, value);
            if (name == null) return string.Empty;
            // 获取枚举字段。
            var fieldInfo = enumType.GetField(name);
            if (fieldInfo == null) return string.Empty;
            // 获取描述的属性。
            var attr = Attribute.GetCustomAttribute(fieldInfo,
                typeof(DescriptionAttribute), false) as DescriptionAttribute;
            return attr != null ? attr.Description : string.Empty;
        }

        /// <summary>
        /// desc:枚举辅助类
        /// date:2015年12月30日
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string ConvertEnumToOption(Type enumType, int? enumValue)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (Enum item in Enum.GetValues(enumType))
            {
                sb.AppendFormat("<option value='{0}' {1}>{2}</option>",
                    Convert.ToInt32(item),
                    enumValue.HasValue && Convert.ToInt32(item) == enumValue.Value ? "selected" : "",
                    GetDescription(item));
            }
            return sb.ToString();
        }
    }
}
