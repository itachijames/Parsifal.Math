using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math
{
    public class MathUtilHelper
    {
        /// <summary>
        /// 是否有重复项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns>有相同项时返回true;否则false</returns>
        public static bool HaveRepeated<T>(params T[] values) where T : IEquatable<T>
        {
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = i + 1; j < values.Length; j++)
                {
                    if (values[i].Equals(values[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 判断两个集合是否相同
        /// </summary>
        /// <remarks>具有相同元素,不论顺序</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">集合A</param>
        /// <param name="second">集合B</param>
        /// <returns>相同时返回true,否则false</returns>
        public static bool IsEquivalent<T>(IEnumerable<T> first, IEnumerable<T> second)
            where T : IEquatable<T>
        {
            if (ReferenceEquals(first, second))
                return true;
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Count() == second.Count()
                && !first.Except(second).Any();
        }
    }
}
