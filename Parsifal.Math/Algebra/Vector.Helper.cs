namespace Parsifal.Math.Algebra
{
    public partial class Vector
    {
        /// <summary>
        /// 获取值(不进行边界校验)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal double Get(int index)
        {
            return _elements[index];
        }
        /// <summary>
        /// 设置值(不进行边界校验)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        internal void Set(int index, double value)
        {
            _elements[index] = value;
        }
        private void CheckRange(int index)
        {
            if (index < 0 || index >= _elements.Length)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(index));
        }
        /// <summary>是否使用并行</summary>
        /// <remarks>用于指示在使用<b>原生算法</b>时是否使用并行运算</remarks>
        /// <returns>应使用返回true;否则false</returns>
        private bool ShouldNotUseParallel()
        {
            return _elements.Length < 1024;
        }
        //private static Vector Convert(Vector source, Func<double, double> func)
        //{
        //    double[] result = new double[source._total];
        //    for (int i = 0; i < result.Length; i++)
        //    {
        //        result[i] = func(source.Get(i));
        //    }
        //    return result;
        //}
        private static void CheckSameDimension(Vector left, Vector right)
        {
            if (left._elements.Length != right._elements.Length)
                ThrowHelper.ThrowDimensionDontMatchException(left, right);
        }
    }
}
