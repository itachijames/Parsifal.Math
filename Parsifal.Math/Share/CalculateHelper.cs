namespace Parsifal.Math
{
    using Math = System.Math;
    /// <summary>
    /// 计算辅助
    /// </summary>
    public static class CalculateHelper
    {
        /// <summary>
        /// (默认)精度误差
        /// </summary>
        public const double AccuracyError = 1.0e-9D;
        /// <summary>
        /// 1D减去产生与1D不同结果的最小正数，大小: 2^-53 
        /// </summary>
        /// <remarks>
        /// (1 - NegativeMachineEpsilon) &lt; 1 且
        /// (1 + NegativeMachineEpsilon) == 1
        /// </remarks>
        public const double NegativeMachineEpsilon = 1.1102230246251565e-16D;
        /// <summary>
        /// 1D加上产生与1D不同结果的最小正数，大小：2*2^-53
        /// </summary>
        /// <remarks>
        /// (1 - PositiveDoublePrecision) &lt; 1 且
        /// (1 + PositiveDoublePrecision) &gt; 1
        /// </remarks>
        public const double PositiveMachineEpsilon = 2D * NegativeMachineEpsilon;

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <param name="maxAbsoluteError">最大绝对误差</param>
        /// <returns>如果相等则返回true;否则false</returns>
        public static bool IsEqual(this double value, double other, double maxAbsoluteError = AccuracyError)
        {
            if (double.IsNaN(value) || double.IsNaN(other))
                return false;
            if (double.IsInfinity(value) || double.IsInfinity(other))
                return value == other;
            return Math.Abs(value - other) < maxAbsoluteError;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        internal static bool IsEqualWithoutCheck(this double value, double other)
        {
            return Math.Abs(value - other) < AccuracyError;
        }
        /// <summary>
        /// 是否小于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsLess(this double value, double other)
        {
            if (double.IsNaN(value) || double.IsNaN(other))
                return false;
            return CompareTo(value, other) < 0;
        }
        /// <summary>
        /// 是否大于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsGreater(this double value, double other)
        {
            if (double.IsNaN(value) || double.IsNaN(other))
                return false;
            return CompareTo(value, other) > 0;
        }
        /// <summary>
        /// 是否小于等于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsLessOrEqual(this double value, double other)
        {
            if (double.IsNaN(value) || double.IsNaN(other))
                return false;
            return CompareTo(value, other) <= 0;
        }
        /// <summary>
        /// 是否大于等于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsGreaterOrEqual(this double value, double other)
        {
            if (double.IsNaN(value) || double.IsNaN(other))
                return false;
            return CompareTo(value, other) >= 0;
        }
        /// <summary>
        /// 是否为0
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsZero(this double value)
        {
            return Math.Abs(value) <= AccuracyError;
        }
        /// <summary>
        /// 数量级
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Magnitude(this double value)
        {
            if (value.Equals(0.0))
                return 0;
            double magnitude = Math.Log10(Math.Abs(value));
            var truncated = (int)Math.Truncate(magnitude);
            return magnitude < 0d && truncated != magnitude
                ? truncated - 1
                : truncated;
        }
        /// <summary>
        /// 判断两数是否符号相同
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns>同号时返回true;异号返回false</returns>
        public static bool IsSameSign(this int value, int other)
        {
            return (value ^ other) >= 0;
        }
        /// <summary>
        /// 判断两数是否符号相同
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns>同号时返回true;异号返回false</returns>
        public static bool IsSameSign(this double value, double other)
        {
            if (value.IsZero())
                return other.IsZero();
            else if (other.IsZero())
                return false;
            else//两数皆不为0
                return Math.Sign(value) * Math.Sign(other) >= 0;
        }

        private static int CompareTo(double first, double second)
        {
            if (first.IsEqual(second))
                return 0;
            return first.CompareTo(second);
        }
    }
}
