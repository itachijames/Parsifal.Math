using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math.Algebra
{
    public class VectorCreator
    {
        /// <summary>
        /// 创建指定元素的向量
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="initFunc">元素初始化方法</param>
        public static Vector CreateWithSpecify(int length, Func<int, double> initFunc)
        {
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(length));
            if (initFunc is null)
                ThrowHelper.ThrowArgumentNullException(nameof(initFunc));
            double[] data = new double[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = initFunc(i);
            }
            return new Vector(data);
        }
        /// <summary>
        /// 根据迭代器创建向量
        /// </summary>
        /// <param name="elements">各项元素</param>
        public static Vector CreateByEnumerable(IEnumerable<double> elements)
        {
            return new Vector(elements as double[] ?? elements.ToArray());
        }
    }
}
