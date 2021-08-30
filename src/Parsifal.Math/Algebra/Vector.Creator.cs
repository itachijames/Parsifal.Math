using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math.Algebra
{
    partial class Vector<T>
    {
        /// <summary>
        /// 创建指定元素的向量
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="initFunc">元素初始化方法</param>
        public Vector<T> CreateWithSpecify(int length, Func<int, T> initFunc)
        {
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(length));
            if (initFunc is null)
                ThrowHelper.ThrowArgumentNullException(nameof(initFunc));
            T[] data = new T[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = initFunc(i);
            }
            return new Vector<T>(data);
        }
        /// <summary>
        /// 根据迭代器创建向量
        /// </summary>
        /// <param name="elements">各项元素</param>
        public Vector<T> CreateByEnumerable(IEnumerable<T> elements)
        {
            if (elements is null)
                ThrowHelper.ThrowArgumentNullException(nameof(elements));
            return new Vector<T>(elements as T[] ?? elements.ToArray());
        }
    }
}
