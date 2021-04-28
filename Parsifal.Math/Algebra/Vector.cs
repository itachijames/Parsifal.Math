using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Parsifal.Math.Algebra
{
    using Math = System.Math;
    /// <summary>
    /// 向量
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Vector ({Dimension})")]
    public sealed partial class Vector : IEnumerable<double>, IEquatable<Vector>, ICloneable, IFormattable
    {
        #region field
        private const int DoubleSize = sizeof(double);
        private readonly double[] _elements;
        #endregion

        #region property
        internal double[] Storage { get => _elements; }
        public double this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckRange(index);
                return Get(index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckRange(index);
                Set(index, value);
            }
        }
        /// <summary>
        /// 维数
        /// </summary>
        public int Dimension => _elements.Length;
        #endregion

        #region constructor
        /// <summary>
        /// 创建指定长度的向量
        /// </summary>
        /// <param name="length">数值长度</param>
        public Vector(int length)
        {
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(length));
            _elements = new double[length];
        }
        /// <summary>
        /// 创建指定元素的向量
        /// </summary>
        /// <param name="element">初始元素</param>
        public Vector(double[] element)
            : this(element, true) { }
        private Vector(double[] element, bool isCopy)
        {
            if (element is null)
                ThrowHelper.ThrowArgumentNullException(nameof(element));
            if (isCopy)
            {
                _elements = new double[element.Length];
                Buffer.BlockCopy(element, 0, _elements, 0, element.Length * DoubleSize);
            }
            else
            {
                _elements = element;
            }
        }
        #endregion

        #region IEnumerable
        public IEnumerator<double> GetEnumerator()
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
        public bool Equals(Vector other)
        {
            if (other is null)
                return false;
            if (_elements.Length != other._elements.Length)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            for (int i = 0; i < _elements.Length; i++)
            {
                if (Get(i).Equals(other.Get(i)))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region ICloneable
        object ICloneable.Clone()
        {
            return Clone();
        }
        public Vector Clone()
        {
            return new Vector(_elements, true);
        }
        #endregion

        #region IFormattable
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Join(' ', _elements.Select(p => p.ToString(format, formatProvider)));
        }
        #endregion

        #region public
        /// <summary>
        /// 向量点积/内积(Inner Product, dot product)
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>数量积</returns>
        public double DotProduct(Vector vector)
        {
            return DotProduct(this, vector);
        }
        public override bool Equals(object obj) => obj is Vector vector && Equals(vector);
        public override int GetHashCode() => _elements.GetHashCode();
        public override string ToString() => ToString("G", CultureInfo.CurrentCulture);
        #endregion

        #region Norm
        /// <summary>
        /// 0范数
        /// </summary>
        /// <remarks>非0元素个数</remarks>
        public double ZeroNorm()
        {
            var count = 0;
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!Get(i).IsZero())
                    count++;
            }
            return count;
        }
        /// <summary>
        /// 1范数
        /// </summary>
        /// <remarks>元素绝对值之和</remarks>
        public double OneNorm()
        {
            double result = 0d;
            for (int i = 0; i < _elements.Length; i++)
            {
                result += Math.Abs(Get(i));
            }
            return result;
        }
        /// <summary>
        /// 2范数
        /// </summary>
        /// <remarks>元素绝对值平方和的开方</remarks>
        public double TwoNorm()
        {
            return Math.Sqrt(DotProduct(this, this));
        }
        /// <summary>
        /// P阶范数
        /// </summary>
        /// <param name="p">阶数</param>
        public double Norm(double p)
        {
            if (p.IsLess(0d))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(p));
            if (p.IsEqual(1d))
                return OneNorm();
            if (p.IsEqual(2d))
                return TwoNorm();
            //元素绝对值的p次方之和的1/p次幂
            double result = 0d;
            for (var i = 0; i < _elements.Length; i++)
            {
                result += Math.Pow(Math.Abs(Get(i)), p);
            }
            return Math.Pow(result, 1.0 / p);
        }
        #endregion

        #region VectorOperation
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
        public void CoerceZero()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (Get(i).IsZero())
                {
                    Set(i, 0d);
                }
            }
        }
        /// <summary>
        /// 数值归零
        /// </summary>
        /// <param name="zeroPredicate">为0判定</param>
        public void CoerceZero(Predicate<double> zeroPredicate)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (zeroPredicate(Get(i)))
                {
                    Set(i, 0d);
                }
            }
        }
        /// <summary>
        /// 转为行矩阵
        /// </summary>
        public Matrix ToRowMatrix()
        {
            var items = new double[_elements.Length];
            Buffer.BlockCopy(_elements, 0, items, 0, items.Length * DoubleSize);
            return new Matrix(1, _elements.Length, items, false);
        }
        /// <summary>
        /// 转为列矩阵
        /// </summary>
        public Matrix ToColumnMatrix()
        {
            var items = new double[_elements.Length];
            Buffer.BlockCopy(_elements, 0, items, 0, items.Length * DoubleSize);
            return new Matrix(_elements.Length, 1, items, false);
        }
        /// <summary>
        /// 转为一维数组
        /// </summary>
        /// <returns></returns>
        public double[] ToArray()
        {
            double[] result = new double[_elements.Length];
            Buffer.BlockCopy(_elements, 0, result, 0, result.Length * DoubleSize);
            return result;
        }
        #endregion
    }
}
