using System;
using System.Numerics;

namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 代数泛型基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AlgebraGenricBase<T> where T : struct, IEquatable<T>
    {

        protected static T Zero { get => default; }
        protected static T One { get => GetOne(); }

        protected AlgebraGenricBase()
        {
            CheckSupportType();
        }

        protected static void CheckSupportType()
        {
            if (typeof(T) != typeof(float) && typeof(T) != typeof(double) && typeof(T) != typeof(System.Numerics.Complex))
                ThrowHelper.ThrowNotSupportedException(ErrorReason.NotSupportType);
        }
        private static T GetOne()
        {
            if (typeof(T) == typeof(float))
                return (T)(object)1f;
            else if (typeof(T) == typeof(double))
                return (T)(object)1d;
            else if (typeof(T) == typeof(Complex))
                return (T)(object)Complex.One;
            else
                throw new NotSupportedException(ErrorReason.NotSupportType);
        }
        protected static bool IsZero(T value)
        {
            if (typeof(T) == typeof(float))
            {
                return (float)(object)value == 0;
            }
            if (typeof(T) == typeof(double))
            {
                return (double)(object)value == 0;
            }
            if (typeof(T) == typeof(Complex))
            {
                return (Complex)(object)value == Complex.Zero;
            }
            else
                throw new NotSupportedException(ErrorReason.NotSupportType);
        }
    }
}
