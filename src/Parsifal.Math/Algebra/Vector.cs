using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 向量
    /// </summary>
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("Vector ({Dimension})")]
    public partial class Vector<T> : AlgebraGenricBase<T>, IEnumerable<T>, IEquatable<Vector<T>>, ICloneable, IFormattable
        where T : struct, IEquatable<T>, IFormattable
    {
        #region field
        private readonly T[] _elements;
        #endregion

        #region property
        internal T[] Storage { get => _elements; }
        public T this[int index]
        {
            get
            {
                CheckIndexRange(index);
                return Get(index);
            }
            set
            {
                CheckIndexRange(index);
                Set(index, value);
            }
        }
        /// <summary>
        /// 维度/项数
        /// </summary>
        public int Count => _elements.Length;
        #endregion

        #region constructor
        public Vector(int length)
        {

        }
        /// <summary>
        /// 创建指定元素的向量
        /// </summary>
        /// <param name="elements">初始元素</param>
        internal Vector(T[] elements)
        {
            _elements = elements;
        }
        #endregion

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                yield return Get(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                yield return Get(i);
            }
        }
        #endregion

        #region IEquatable
        public bool Equals(Vector<T> other)
        {
            if (other is null)
                return false;
            if (_elements.Length != other._elements.Length)
                return false;
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!Get(i).Equals(other.Get(i)))
                    return false;
            }
            return true;
        }
        #endregion

        #region ICloneable
        object ICloneable.Clone()
        {
            return Clone();
        }
        public Vector<T> Clone()
        {
            T[] data = new T[_elements.Length];
            Array.Copy(_elements, 0, data, 0, _elements.Length);
            return new Vector<T>(data);
        }
        #endregion

        #region IFormattable
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Join('\t', _elements.Select(p => p.ToString(format, formatProvider)));
        }
        #endregion

        #region BCL
        public override bool Equals(object obj) => obj is Vector<T> vector && Equals(vector);
        public override int GetHashCode() => _elements.GetHashCode();
        public override string ToString() => ToString(UtilityHelper.DigitalFormat, System.Globalization.CultureInfo.CurrentCulture);
        #endregion

        #region Norm
        ///// <summary>
        ///// 0范数
        ///// </summary>
        ///// <remarks>非0元素个数</remarks>
        //public double ZeroNorm()
        //{
        //    int count = 0;
        //    for (int i = 0; i < _elements.Length; i++)
        //    {
        //        if (!Get(i).IsZero())
        //            count++;
        //    }
        //    return count;
        //}
        /// <summary>
        /// 1范数
        /// </summary>
        /// <remarks>元素绝对值之和</remarks>
        public T OneNorm()
        {
            throw new NotImplementedException();
            //T result = default;
            //for (int i = 0; i < _elements.Length; i++)
            //{
            //    result += Math.Abs(Get(i));
            //}
            //return result;
        }
        /// <summary>
        /// 2范数
        /// </summary>
        /// <remarks>元素绝对值平方和的开方</remarks>
        public T TwoNorm()
        {
            throw new NotImplementedException();
            //return Math.Sqrt(DotProduct(this, this));
        }
        /// <summary>
        /// P阶范数
        /// </summary>
        /// <param name="p">阶数</param>
        public T Norm(double p)
        {
            throw new NotImplementedException();
            //if (p.IsLess(0d))
            //    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(p));
            //if (p.IsEqual(1d))
            //    return OneNorm();
            //if (p.IsEqual(2d))
            //    return TwoNorm();
            ////元素绝对值的p次方之和的1/p次幂
            //double result = 0d;
            //for (int i = 0; i < _elements.Length; i++)
            //{
            //    result += Math.Pow(Math.Abs(Get(i)), p);
            //}
            //return Math.Pow(result, 1.0 / p);
        }
        #endregion

        #region VectorOperation
        /// <summary>
        /// 获取子向量
        /// </summary>
        /// <param name="index">起始索引</param>
        /// <param name="count">项数</param>
        /// <returns>子向量</returns>
        public Vector<T> GetSubVector(int index, int count)
        {
            if (count < 1)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.NotPositiveParameter);
            if (index < 0 || index + count > _elements.Length)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
            T[] data = new T[count];
            Array.Copy(_elements, index, data, 0, count);
            return new Vector<T>(data);
        }
        /// <summary>
        /// 设置子向量
        /// </summary>
        /// <param name="index">起始索引</param>
        /// <param name="subVector">子向量</param>
        public void SetSubVector(int index, Vector<T> subVector)
        {
            if (subVector is null || subVector._elements.Length > _elements.Length)
                ThrowHelper.ThrowArgumentNullException(nameof(subVector));
            if (index < 0 || index + subVector._elements.Length > _elements.Length)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
            Array.Copy(subVector._elements, 0, _elements, index, subVector._elements.Length);
        }
        /// <summary>
        /// 清空向量
        /// </summary>
        public void Clear()
        {
            Array.Clear(_elements, 0, _elements.Length);
        }
        /// <summary>
        /// 数值归零
        /// </summary>
        /// <param name="zeroPredicate">为0判定</param>
        public void CoerceZero(Predicate<T> zeroPredicate)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (zeroPredicate(Get(i)))
                {
                    Set(i, default);
                }
            }
        }
        /// <summary>
        /// 转为一维数组
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] result = new T[_elements.Length];
            Array.Copy(_elements, 0, result, 0, _elements.Length);
            return result;
        }
        #endregion
    }
}
