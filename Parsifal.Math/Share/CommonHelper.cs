using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math
{
    internal static class CommonHelper
    {
        const int FloatSize = sizeof(float);
        const int DoubleSize = sizeof(double);

        /// <summary>
        /// 拷贝到目标数组(不检查参数，需保证两者长度相同)
        /// </summary>
        public static void CopyToWithoutCheck(this float[] source, float[] target)
        {
            Buffer.BlockCopy(source, 0, target, 0, source.Length * FloatSize);
        }
        /// <summary>
        /// 拷贝到目标数组(不检查参数，需保证两者长度相同)
        /// </summary>
        public static void CopyToWithoutCheck(this double[] source, double[] target)
        {
            Buffer.BlockCopy(source, 0, target, 0, source.Length * DoubleSize);
        }
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
        {
            if (ReferenceEquals(first, second))
                return true;
            if (first is null)
                ThrowHelper.ThrowArgumentNullException(nameof(first));
            if (second is null)
                ThrowHelper.ThrowArgumentNullException(nameof(second));

            return first.Count() == second.Count()
                && !first.Except(second).Any();
        }
    }
}
