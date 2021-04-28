using System;
using System.Diagnostics.CodeAnalysis;

namespace Parsifal.Math
{
    internal static class ThrowHelper
    {
        private const string EmptyString = "";

        /// <summary>
        /// 不支持的操作
        /// </summary>
        [DoesNotReturn]
        public static void ThrowNotSupportedException(string message) => throw new NotSupportedException(message);
        /// <summary>
        /// 参数为空
        /// </summary>
        /// <param name="argumentName">参数名</param>
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string argumentName) => throw new ArgumentNullException(argumentName);
        /// <summary>
        /// 参数非法
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="argumentName">参数名</param>
        [DoesNotReturn]
        public static void ThrowIllegalArgumentException(string message, string argumentName = EmptyString) => throw new ArgumentException(message, argumentName);
        /// <summary>
        /// 参数越界
        /// </summary>
        /// <param name="argumentName">参数名</param>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string argumentName) => throw new ArgumentOutOfRangeException(argumentName);
        /// <summary>
        /// 索引越界
        /// </summary>
        /// <param name="argumentName">参数名</param>
        [DoesNotReturn]
        public static void ThrowIndexOutOfRangeException(string argumentName) => throw new IndexOutOfRangeException($"{ErrorReason.IndexOutOfRange}!参数:{argumentName}");
        /// <summary>
        /// 无穷数
        /// </summary>
        /// <param name="number">异常数</param>
        [DoesNotReturn]
        public static void ThrowInfiniteNumberException(double number) => throw new NotFiniteNumberException(number);
        /// <summary>
        /// 数值溢出
        /// </summary>
        /// <param name="message">错误消息</param>
        [DoesNotReturn]
        public static void ThrowOverflowException(string message) => throw new OverflowException(message);
        /// <summary>
        /// 计算异常
        /// </summary>
        /// <param name="message">错误消息</param>
        [DoesNotReturn]
        public static void ThrowArithmeticException(string message) => throw new ArithmeticException(message);
        /// <summary>
        /// 维度匹配异常
        /// </summary>
        [DoesNotReturn]
        public static void ThrowDimensionDontMatchException(params object[] arguments) => throw new DimensionMatchException(arguments);
    }
}
