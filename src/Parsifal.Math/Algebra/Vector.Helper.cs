using System.Runtime.CompilerServices;

namespace Parsifal.Math.Algebra
{
    partial class Vector<T>
    {
        /// <summary>
        /// 获取值(不进行边界校验)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Get(int index)
        {
            return _elements[index];
        }
        /// <summary>
        /// 设置值(不进行边界校验)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Set(int index, T value)
        {
            _elements[index] = value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckIndexRange(int index)
        {
            if (index < 0 || index >= _elements.Length)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(index));
        }
        private static void CheckSameDimension(Vector<T> left, Vector<T> right)
        {
            if (left._elements.Length != right._elements.Length)
                ThrowHelper.ThrowDimensionDontMatchException(left, right);
        }
    }
}
