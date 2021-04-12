using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Parsifal.Math.Algebra
{
    using Math = System.Math;
    /// <summary>
    /// 矩阵
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Matrix ({Rows} × {Columns})")]
    public sealed partial class Matrix : IEnumerable<double>, IEquatable<Matrix>, ICloneable, IFormattable
    {
        #region field
        private const int DoubleSize = sizeof(double);
        /// <summary>
        /// 元素（按行主序存储）
        /// </summary>
        private readonly double[] _elements;
        private readonly int _rowCount;
        private readonly int _colCount;
        #endregion

        #region property
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows => _rowCount;
        /// <summary>
        /// 列数
        /// </summary>
        public int Columns => _colCount;
        /// <summary>
        /// 元素数量
        /// </summary>
        public int Count => _elements.Length;
        /// <summary>
        /// 是否为方阵
        /// </summary>
        public bool IsSquare => _rowCount == _colCount;
        /// <summary>
        /// 获取/设置元素值
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="colIndex">列索引</param>
        /// <returns></returns>
        public double this[int rowIndex, int colIndex]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckRowIndex(rowIndex);
                CheckColumnIndex(colIndex);
                return Get(rowIndex, colIndex);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckRowIndex(rowIndex);
                CheckColumnIndex(colIndex);
                Set(rowIndex, colIndex, value);
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 创建指定阶数的矩阵
        /// </summary>
        /// <param name="order">阶数</param>
        public Matrix(int order)
            : this(order, order) { }
        /// <summary>
        /// 创建指定行列数的矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        public Matrix(int rows, int columns)
            : this(rows, columns, true) { }
        /// <summary>
        /// 根据二维数组创建矩阵
        /// </summary>
        /// <param name="elements">二维数组</param>
        public Matrix(double[,] elements)
        {
            if (elements is null)
                ThrowHelper.ThrowArgumentNullException(nameof(elements));
            _rowCount = elements.GetLength(0);
            _colCount = elements.GetLength(1);
            var temp = _rowCount * _colCount;
            _elements = new double[temp];
            Buffer.BlockCopy(elements, 0, _elements, 0, temp * DoubleSize);//二维数组默认按行存储，直接复制
        }
        /// <summary>
        /// 根据指定项创建指定行列数的矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="elements">各项元素(按行顺序分布)</param>
        public Matrix(int rows, int columns, double[] elements)
            : this(rows, columns, elements, true) { }
        private Matrix(int rows, int columns, bool check)
        {
            if (check)
            {
                if (rows < 1)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
                if (columns < 1)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
            }
            _rowCount = rows;
            _colCount = columns;
            _elements = new double[rows * columns];
        }
        private Matrix(int rows, int columns, double[] elements, bool isCopy)
        {
            if (isCopy)
            {
                if (rows < 1)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
                if (columns < 1)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
                if (elements is null)
                    ThrowHelper.ThrowArgumentNullException(nameof(elements));
                int tempLength = rows * columns;
                if (tempLength != elements.Length)
                    ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(elements));
                _elements = new double[tempLength];
                Buffer.BlockCopy(elements, 0, _elements, 0, tempLength * DoubleSize);
            }
            else
            {
                _elements = elements;
            }
            _rowCount = rows;
            _colCount = columns;
        }
        #endregion

        #region creater
        /// <summary>
        /// 零矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        /// <returns></returns>
        public static Matrix CreateZero(int order)
        {
            return CreateZero(order, order);
        }
        /// <summary>
        /// 零矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <returns></returns>
        public static Matrix CreateZero(int rows, int columns)
        {
            return new Matrix(rows, columns, true);
        }
        /// <summary>
        /// 对角线矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="init">对角线元素初始化委托</param>
        /// <returns></returns>
        public static Matrix CreateDiagonal(int rows, int columns, Func<int, double> init)
        {
            if (rows < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
            if (columns < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
            if (init is null)
                ThrowHelper.ThrowArgumentNullException(nameof(init));
            var result = new Matrix(rows, columns, false);
            var min = Math.Min(rows, columns);
            for (int i = 0; i < min; i++)
            {
                result.Set(i, i, init(i));
            }
            return result;
        }
        /// <summary>
        /// 单位矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        /// <returns></returns>
        public static Matrix CreateIdentity(int order)
        {
            if (order < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(order));
            var result = new double[order * order];
            for (int i = 0; i < order; i++)
            {
                result[i * (order + 1)] = 1d;
            }
            return new Matrix(order, order, result, false);
        }
        #endregion

        #region IEquatable
        public bool Equals(Matrix other)
        {
            if (other is null)
                return false;
            if (_rowCount != other._rowCount || _colCount != other._colCount)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!Get(i).Equals(other.Get(i)))
                {//依次比较所有元素值
                    return false;
                }
            }
            return true;
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

        #region ICloneable
        object ICloneable.Clone()
        {
            return Clone();
        }
        public Matrix Clone()
        {
            return new Matrix(_rowCount, _colCount, _elements, true);
        }
        #endregion

        #region IFormattable
        public string ToString(string format, IFormatProvider formatProvider)
        {
            int tempCol = _colCount - 1;
            var sb = new StringBuilder();
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < tempCol; j++)
                {
                    sb.Append(Get(i, j).ToString(format, formatProvider)).Append('\t');
                }
                sb.AppendLine(Get(i, tempCol).ToString(format, formatProvider));
            }
            return sb.ToString();
        }
        #endregion

        #region public
        /// <summary>
        /// 是否为对称矩阵
        /// </summary>
        public bool IsSymmetric()
        {
            if (_rowCount != _colCount)
                return false;
            for (var i = 0; i < _rowCount; i++)
            {
                for (var j = i + 1; j < _colCount; j++)
                {
                    if (!Get(i, j).Equals(Get(j, i)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public LU LU()
        {
            return new LU(this);
        }
        public QR QR()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 行列式
        /// </summary>
        public double Determinant()
        {
            CheckSquareMatrix(this);
            return LU().Determinant();
        }
        /// <summary>
        /// 余子式
        /// </summary>
        /// <param name="row">元素行索引</param>
        /// <param name="col">元素列索引</param>
        public double Cofactor(int row, int col)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 条件数
        /// </summary>
        public double ConditionNumber()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 矩阵秩
        /// </summary>
        public int Rank()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 迹
        /// </summary>
        /// <remarks>主对角线元素之和</remarks>
        public double Trace()
        {
            CheckSquareMatrix(this);
            var result = 0d;
            for (int i = 0; i < _rowCount; i++)
            {
                result += Get(i, i);
            }
            return result;
        }
        /// <summary>
        /// 逆矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            CheckSquareMatrix(this);
            return LU().Inverse();
        }
        /// <summary>
        /// 伪逆矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix PseudoInverse()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            var result = new Matrix(_colCount, _rowCount, false);
            for (int i = 0; i < result._rowCount; i++)
            {
                for (int j = 0; j < result._colCount; j++)
                {
                    result.Set(i, j, Get(j, i));
                }
            }
            return result;
        }
        /// <summary>
        /// 幂矩阵
        /// </summary>
        /// <param name="exponent">幂次</param>
        /// <returns></returns>
        public Matrix Power(int exponent)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 下三角矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix LowerTriangle()
        {
            var result = new Matrix(_rowCount, _colCount, false);
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j <= i && j < _colCount; j++)
                {
                    result.Set(i, j, Get(i, j));
                }
            }
            return result;
        }
        /// <summary>
        /// 上三角矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix UpperTriangle()
        {
            var result = new Matrix(_rowCount, _colCount, false);
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = i; j < _colCount; j++)
                {
                    result.Set(i, j, Get(i, j));
                }
            }
            return result;
        }
        /// <summary>
        /// 核
        /// </summary>
        /// <returns></returns>
        public double[] Kernel()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 象
        /// </summary>
        /// <returns></returns>
        public double[] Image()
        {
            throw new NotImplementedException();
        }
        public override bool Equals(object obj) => obj is Matrix mat && Equals(mat);
        public override int GetHashCode() => HashCode.Combine(_rowCount, _colCount, _elements.GetHashCode());
        public override string ToString() => ToString("G", CultureInfo.CurrentCulture);
        #endregion

        #region Norm
        /// <summary>
        /// L1范数
        /// </summary>
        /// <remarks>列向量绝对值之和的最大值</remarks>
        public double L1Norm()
        {
            var norm = 0d;
            for (int i = 0; i < _colCount; i++)
            {
                var temp = 0d;
                for (int j = 0; j < _rowCount; j++)
                {
                    temp += Math.Abs(Get(j, i));
                }
                norm = Math.Max(temp, norm);
            }
            return norm;
        }
        /// <summary>
        /// L2范数
        /// </summary>
        /// <remarks>(AT)*A 的最大特征值开方 √(λmax)</remarks>
        public double L2Norm()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 无穷范数
        /// </summary>
        /// <remarks>行向量绝对值之和的最大值</remarks>
        public double InfinityNorm()
        {
            double norm = 0d;
            for (int i = 0; i < _rowCount; i++)
            {
                double temp = 0d;
                for (int j = 0; j < _colCount; j++)
                {
                    temp += Math.Abs(Get(i, j));
                }
                norm = Math.Max(temp, norm);
            }
            return norm;
        }
        /// <summary>
        /// F范数
        /// </summary>
        /// <remarks>所有元素平方和的开方</remarks>
        public double FrobeniusNorm()
        {
            double temp = 0d;
            for (int i = 0; i < _elements.Length; i++)
            {
                temp += Math.Pow(Get(i), 2);
            }
            return Math.Sqrt(temp);
        }
        /// <summary>
        /// 核范数
        /// </summary>
        /// <remarks>奇异值之和</remarks>
        public double NuclearNorm()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region MatrixOperation
        /// <summary>
        /// 获取行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public IEnumerable<double> GetRow(int rowIndex)
        {
            CheckRowIndex(rowIndex);
            for (int i = 0; i < _colCount; i++)
            {
                yield return Get(rowIndex, i);
            }
        }
        /// <summary>
        /// 获取行向量
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public Vector GetRowVector(int rowIndex)
        {
            CheckRowIndex(rowIndex);
            var result = new double[_colCount];
            Buffer.BlockCopy(_elements, rowIndex * _colCount * DoubleSize, result, 0, _colCount * DoubleSize);
            return result;
        }
        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public IEnumerable<double> GetColumn(int columnIndex)
        {
            CheckColumnIndex(columnIndex);
            for (int i = 0; i < _rowCount; i++)
            {
                yield return Get(i, columnIndex);
            }
        }
        /// <summary>
        /// 获取列向量
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public Vector GetColumnVector(int columnIndex)
        {
            CheckColumnIndex(columnIndex);
            var result = new double[_rowCount];
            for (int i = 0; i < _rowCount; i++)
            {
                result[i] = Get(i, columnIndex);
            }
            return result;
        }
        /// <summary>
        /// 获取子矩阵
        /// </summary>
        /// <param name="rowIndex">起始行索引</param>
        /// <param name="rCount">行数</param>
        /// <param name="columnIndex">起始列索引</param>
        /// <param name="cCount">列数</param>
        /// <returns>子矩阵</returns>
        public Matrix GetSubMatrix(int rowIndex, int rCount, int columnIndex, int cCount)
        {
            if (rCount <= 0 || cCount <= 0)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.NotPositiveParameter);
            if (rowIndex < 0 || rowIndex + rCount > _rowCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowIndex));
            if (columnIndex < 0 || columnIndex + cCount > _colCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnIndex));
            var result = new double[rCount * cCount];
            for (int i = 0; i < rCount; i++)
            {
                Buffer.BlockCopy(_elements, ((rowIndex + i) * _colCount + columnIndex) * DoubleSize,
                    result, i * cCount * DoubleSize,
                    cCount * DoubleSize);
            }
            return new Matrix(rCount, cCount, result, false);
        }
        /// <summary>
        /// 设置行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="row">行元素</param>
        public void SetRow(int rowIndex, double[] row)
        {
            CheckRowIndex(rowIndex);
            if (row is null)
                ThrowHelper.ThrowArgumentNullException(nameof(row));
            if (row.Length != _colCount)
                ThrowHelper.ThrowDimensionDontMatchException();
            Buffer.BlockCopy(row, 0, _elements, rowIndex * _colCount * DoubleSize, _colCount * DoubleSize);
        }
        /// <summary>
        /// 设置行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="row">行向量</param>
        public void SetRow(int rowIndex, Vector row)
        {
            CheckRowIndex(rowIndex);
            if (row is null)
                ThrowHelper.ThrowArgumentNullException(nameof(row));
            if (row.Dimension != _colCount)
                ThrowHelper.ThrowDimensionDontMatchException();
            for (int i = 0; i < _colCount; i++)
            {
                Set(rowIndex, i, row.Get(i));
            }
        }
        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="column">列元素</param>
        public void SetColumn(int columnIndex, double[] column)
        {
            CheckColumnIndex(columnIndex);
            if (column is null)
                ThrowHelper.ThrowArgumentNullException(nameof(column));
            if (column.Length != _rowCount)
                ThrowHelper.ThrowDimensionDontMatchException();
            for (int i = 0; i < _rowCount; i++)
            {
                Set(i, columnIndex, column[i]);
            }
        }
        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="column">列向量</param>
        public void SetColumn(int columnIndex, Vector column)
        {
            CheckColumnIndex(columnIndex);
            if (column is null)
                ThrowHelper.ThrowArgumentNullException(nameof(column));
            if (column.Dimension != _rowCount)
                ThrowHelper.ThrowDimensionDontMatchException();
            for (int i = 0; i < _rowCount; i++)
            {
                Set(i, columnIndex, column.Get(i));
            }
        }
        /// <summary>
        /// 添加到下方
        /// </summary>
        /// <param name="matrix">待添加矩阵</param>
        public Matrix ConcatenateBelow(Matrix matrix)
        {
            CheckSameColumn(this, matrix);
            var rows = _rowCount + matrix._rowCount;
            var result = new double[rows * _colCount];
            Buffer.BlockCopy(_elements, 0, result, 0, _elements.Length * DoubleSize);
            Buffer.BlockCopy(matrix._elements, 0, result, _elements.Length * DoubleSize, matrix._elements.Length * DoubleSize);
            return new Matrix(rows, _colCount, result, false);
        }
        /// <summary>
        /// 添加到右方
        /// </summary>
        /// <param name="matrix">待添加矩阵</param>
        public Matrix ConcatenateRight(Matrix matrix)
        {
            CheckSameRow(this, matrix);
            var cols = _colCount + matrix._colCount;
            var result = new double[_rowCount * cols];
            for (int i = 0; i < _rowCount; i++)
            {
                Buffer.BlockCopy(_elements, i * _colCount * DoubleSize, result, i * cols * DoubleSize, _colCount * DoubleSize);
                Buffer.BlockCopy(matrix._elements, i * matrix._colCount * DoubleSize, result, i * _colCount * DoubleSize, matrix._colCount * DoubleSize);
            }
            return new Matrix(_rowCount, cols, result, false);
        }
        /// <summary>
        /// 清空矩阵
        /// </summary>
        public void Clear()
        {
            Array.Clear(_elements, 0, _elements.Length);
        }
        /// <summary>
        /// 清空行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public void ClearRow(int rowIndex)
        {
            CheckRowIndex(rowIndex);
            Array.Clear(_elements, rowIndex * _colCount, _colCount);
        }
        /// <summary>
        /// 清空列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public void ClearColumn(int columnIndex)
        {
            CheckColumnIndex(columnIndex);
            for (int i = 0; i < _rowCount; i++)
            {
                Set(i, columnIndex, 0d);
            }
        }
        /// <summary>
        /// 将子矩阵置零
        /// </summary>
        /// <param name="rowIndex">起始行索引</param>
        /// <param name="rCount">行数</param>
        /// <param name="columnIndex">起始列索引</param>
        /// <param name="cCount">列数</param>
        public void ClearSubMatrix(int rowIndex, int rCount, int columnIndex, int cCount)
        {
            if (rCount > 0 && cCount > 0)
            {
                if (rowIndex < 0 || rowIndex + rCount > _rowCount)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowIndex));
                if (columnIndex < 0 || columnIndex + cCount > _colCount)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnIndex));
                for (int i = rowIndex; i < rowIndex + rCount; i++)
                {
                    Array.Clear(_elements, i * _colCount + columnIndex, cCount);
                }
            }
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
        /// 转为一维数组
        /// </summary>
        public double[] ToArray()
        {
            double[] result = new double[_elements.Length];
            Buffer.BlockCopy(_elements, 0, result, 0, result.Length * DoubleSize);
            return result;
        }
        /// <summary>
        /// 转为二维数组
        /// </summary>
        public double[,] To2DArray()
        {
            double[,] result = new double[_rowCount, _colCount];
            Buffer.BlockCopy(_elements, 0, result, 0, result.Length * DoubleSize);
            return result;
        }
        /// <summary>
        /// 转为交错数组
        /// </summary>
        public double[][] ToJaggedArray()
        {
            double[][] result = new double[_rowCount][];
            for (int i = 0; i < _rowCount; i++)
            {
                result[i] = new double[_colCount];
                Buffer.BlockCopy(_elements, i * _colCount * DoubleSize, result[i], 0, _colCount * DoubleSize);
            }
            return result;
        }
        #endregion
    }
}
