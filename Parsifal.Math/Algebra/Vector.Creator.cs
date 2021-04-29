using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math.Algebra
{
    public partial class Vector
    {
        /// <summary>
        /// 创建指定长度向量
        /// </summary>
        /// <param name="length">长度</param>
        public static Vector Create(int length)
        {
            return new Vector(length);
        }
        /// <summary>
        /// 创建指定元素的向量
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="init">元素初始化方法</param>
        public static Vector Create(int length, Func<int, double> init)
        {
            if (init is null)
                ThrowHelper.ThrowArgumentNullException(nameof(init));
            double[] data = new double[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = init(i);
            }
            return new Vector(data);
        }
        /// <summary>
        /// 创建随机元素的向量
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="minimum">最小值</param>
        /// <param name="maximum">最大值</param>
        public static Vector CreateRandom(int length, double minimum, double maximum)
        {
            if (maximum < minimum)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(maximum));
            double range = maximum - minimum;
            var random = new Random(Guid.NewGuid().GetHashCode());
            double[] data = new double[length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = random.NextDouble() * range + minimum;
            }
            return new Vector(data);
        }
        /// <summary>
        /// 根据数组创建向量
        /// </summary>
        /// <param name="array">元素值数组</param>
        public static Vector CreateByArray(double[] array)
        {
            return new Vector(array);
        }
        /// <summary>
        /// 根据迭代器创建向量
        /// </summary>
        /// <param name="data">元素值</param>
        public static Vector CreateByEnumerable(IEnumerable<double> data)
        {
            return CreateByArray(data as double[] ?? data.ToArray());
        }
    }
}
