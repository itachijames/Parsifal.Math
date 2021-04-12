using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Parsifal.Math
{
    public static class MathUtilExtension
    {
        /// <summary>
        /// 获取枚举<see cref="DescriptionAttribute"/>值
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstend">没有Description属性时是否用枚举名代替</param>
        /// <returns></returns>
        public static string GetDescription(this Enum value, bool nameInstend = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
                return null;
            var attribute = Attribute.GetCustomAttribute(type.GetField(name), typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
                return name;
            return attribute?.Description;
        }
        /// <summary>非空项</summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">当前项</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) where T : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return source.Where(x => x != null);
        }
    }
}
