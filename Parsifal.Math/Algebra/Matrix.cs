﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Parsifal.Math.Algebra
{
    using Math = System.Math;
    /// <summary>
    /// 矩阵
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Matrix ({Rows} × {Columns})")]
    public partial class Matrix : IEnumerable<double>, IEquatable<Matrix>, ICloneable, IFormattable
    {
        #region field
        private const int DoubleSize = sizeof(double);
        /// <summary>
        /// 元素
        /// </summary>
        private readonly double[] _elements;
        private readonly MatrixMajorOrder _storageOrder;
        private readonly int _rowCount;
        private readonly int _colCount;
        #endregion

        #region property
        internal double[] Storage { get => _elements; }
        internal MatrixMajorOrder StorageOrder { get => _storageOrder; }
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
            get
            {
                CheckRowIndex(rowIndex);
                CheckColumnIndex(colIndex);
                return Get(rowIndex, colIndex);
            }
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
        {
            CheckValidRowAndColumn(rows, columns);
            _rowCount = rows;
            _colCount = columns;
            _storageOrder = MatrixMajorOrder.Row;
            _elements = new double[rows * columns];
        }
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
            int temp = _rowCount * _colCount;
            _storageOrder = MatrixMajorOrder.Row;
            _elements = new double[temp];
            //二维数组默认按行存储，直接复制
            Buffer.BlockCopy(elements, 0, _elements, 0, temp * DoubleSize);
        }
        /// <summary>
        /// 根据指定项创建指定行列数的矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="elements">各项元素(按行顺序分布)</param>
        /// <param name="elementOrder"></param>
        public Matrix(int rows, int columns, double[] elements, MatrixMajorOrder elementOrder = MatrixMajorOrder.Row)
            : this(rows, columns, elements, elementOrder, true) { }
        internal Matrix(int rows, int columns, double[] elements, MatrixMajorOrder dataOrder, bool check)
        {
            if (check)
            {
                CheckValidRowAndColumn(rows, columns);
                if (elements is null)
                    ThrowHelper.ThrowArgumentNullException(nameof(elements));
                if (rows * columns != elements.Length)
                    ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(elements));
            }
            _rowCount = rows;
            _colCount = columns;
            _storageOrder = dataOrder;
            _elements = elements;
        }
        #endregion

        #region IEquatable
        public bool Equals(Matrix other)
        {
            return this == other;
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
            double[] date = new double[_elements.Length];
            Buffer.BlockCopy(_elements, 0, date, 0, _elements.Length * DoubleSize);
            return new Matrix(_rowCount, _colCount, date, _storageOrder, false);
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
        public override bool Equals(object obj) => obj is Matrix mat && Equals(mat);
        public override int GetHashCode() => HashCode.Combine(_rowCount, _colCount, _elements.GetHashCode());
        public sealed override string ToString() => ToString("G", CultureInfo.CurrentCulture);
        /// <summary>
        /// 是否为对称矩阵
        /// </summary>
        public bool IsSymmetric()
        {
            if (_rowCount != _colCount)
                return false;
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = i + 1; j < _colCount; j++)
                {
                    if (!Get(i, j).IsEqual(Get(j, i)))
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
        public Svd Svd()
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
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <returns>指定元素的代数余子式</returns>
        public double Cofactor(int rowIndex, int columnIndex)
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
        /// <summary>迹</summary>
        /// <remarks>主对角线元素之和</remarks>
        public double Trace()
        {
            CheckSquareMatrix(this);
            double result = 0d;
            for (int i = 0; i < _rowCount; i++)
            {
                result += Get(i, i);
            }
            return result;
        }
        /// <summary>
        /// 求逆矩阵
        /// </summary>
        /// <returns>逆矩阵</returns>
        public Matrix Inverse()
        {
            CheckSquareMatrix(this);
            return LU().Inverse();
        }
        /// <summary>
        /// 伪逆矩阵
        /// </summary>
        public Matrix PseudoInverse()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取转置矩阵
        /// </summary>
        /// <returns>转置矩阵</returns>
        public Matrix Transpose()
        {
            var result = new Matrix(_colCount, _rowCount);
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
        public Matrix LowerTriangle()
        {
            var result = new Matrix(_rowCount, _colCount);
            //todo 用Buffer.BlockCopy实现
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
        public Matrix UpperTriangle()
        {
            var result = new Matrix(_rowCount, _colCount);
            //todo 用Buffer.BlockCopy实现
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
        public double[] Kernel()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 象
        /// </summary>
        public double[] Image()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Norm
        /// <summary>
        /// 1范数
        /// </summary>
        /// <remarks>列向量绝对值之和的最大值</remarks>
        public double OneNorm()
        {
            double norm = 0d;
            for (int i = 0; i < _colCount; i++)
            {
                double temp = 0d;
                for (int j = 0; j < _rowCount; j++)
                {
                    temp += Math.Abs(Get(j, i));
                }
                norm = Math.Max(temp, norm);
            }
            return norm;
        }
        /// <summary>
        /// 2范数
        /// </summary>
        /// <remarks>(AT)*A 的最大特征值开方 √(λmax)</remarks>
        public double TwoNorm()
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
        /// 获取列数组
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public double[] GetRowArray(int rowIndex)
        {
            CheckRowIndex(rowIndex);
            double[] result = new double[_colCount];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                Buffer.BlockCopy(_elements, rowIndex * _colCount * DoubleSize, result, 0, _colCount * DoubleSize);
            }
            else
            {
                //todo
            }
            return result;
        }
        /// <summary>
        /// 获取行向量
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <returns>行向量</returns>
        public Vector GetRowVector(int rowIndex)
        {
            return GetRowArray(rowIndex);
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
        /// 获取列数组
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public double[] GetColumnArray(int columnIndex)
        {
            CheckColumnIndex(columnIndex);
            double[] result = new double[_rowCount];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                for (int i = 0; i < _rowCount; i++)
                {
                    result[i] = Get(i, columnIndex);
                }
            }
            else
            {
                //todo
            }
            return result;
        }
        /// <summary>
        /// 获取列向量
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <returns>列向量</returns>
        public Vector GetColumnVector(int columnIndex)
        {
            return GetColumnArray(columnIndex);
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
            if (rCount < 1 || cCount < 1)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.NotPositiveParameter);
            if (rowIndex < 0 || rowIndex + rCount > _rowCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowIndex));
            if (columnIndex < 0 || columnIndex + cCount > _colCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnIndex));
            double[] data = new double[rCount * cCount];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                for (int i = 0; i < rCount; i++)
                {
                    Buffer.BlockCopy(_elements, ((rowIndex + i) * _colCount + columnIndex) * DoubleSize,
                        data, i * cCount * DoubleSize,
                        cCount * DoubleSize);
                }
            }
            else
            {
                //todo
            }
            return new Matrix(rCount, cCount, data, _storageOrder, false);
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
                ThrowHelper.ThrowDimensionDontMatchException(this, row);
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                Buffer.BlockCopy(row, 0, _elements, rowIndex * _colCount * DoubleSize, _colCount * DoubleSize);
            }
            else
            {
                //todo
            }
        }
        /// <summary>
        /// 设置行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="row">行向量</param>
        public void SetRow(int rowIndex, Vector row)
        {
            SetRow(rowIndex, row.Storage);
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
                ThrowHelper.ThrowDimensionDontMatchException(this, column);
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                for (int i = 0; i < _rowCount; i++)
                {
                    Set(i, columnIndex, column[i]);
                }
            }
            else
            {
                //todo
            }
        }
        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="column">列向量</param>
        public void SetColumn(int columnIndex, Vector column)
        {
            SetColumn(columnIndex, column.Storage);
        }
        /// <summary>
        /// 设置子矩阵
        /// </summary>
        /// <param name="rowIndex">起始行索引</param>
        /// <param name="columnIndex">起始列索引</param>
        /// <param name="subMatrix">子矩阵</param>
        public void SetSubMatrix(int rowIndex, int columnIndex, Matrix subMatrix)
        {
            if (subMatrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(subMatrix));
            if (rowIndex < 0 || rowIndex + subMatrix._rowCount > _rowCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowIndex));
            if (columnIndex < 0 || columnIndex + subMatrix._colCount > _colCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnIndex));
            //todo
        }
        /// <summary>
        /// 添加到下方
        /// </summary>
        /// <param name="matrix">待添加矩阵</param>
        public Matrix ConcatenateBelow(Matrix matrix)
        {
            CheckSameColumn(this, matrix);
            int rows = _rowCount + matrix._rowCount;
            double[] data = new double[rows * _colCount];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                Buffer.BlockCopy(_elements, 0, data, 0, _elements.Length * DoubleSize);
                Buffer.BlockCopy(matrix._elements, 0, data, _elements.Length * DoubleSize, matrix._elements.Length * DoubleSize);
            }
            else
            {
                //todo
            }
            return new Matrix(rows, _colCount, data, _storageOrder, false);
        }
        /// <summary>
        /// 添加到右方
        /// </summary>
        /// <param name="matrix">待添加矩阵</param>
        public Matrix ConcatenateRight(Matrix matrix)
        {
            CheckSameRow(this, matrix);
            int cols = _colCount + matrix._colCount;
            double[] result = new double[_rowCount * cols];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                for (int i = 0; i < _rowCount; i++)
                {
                    Buffer.BlockCopy(_elements, i * _colCount * DoubleSize, result, i * cols * DoubleSize, _colCount * DoubleSize);
                    Buffer.BlockCopy(matrix._elements, i * matrix._colCount * DoubleSize, result, i * _colCount * DoubleSize, matrix._colCount * DoubleSize);
                }
            }
            else
            {
                //todo
            }
            return new Matrix(_rowCount, cols, result, _storageOrder, false);
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
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                Array.Clear(_elements, rowIndex * _colCount, _colCount);
            }
            else
            {
                //todo
            }
        }
        /// <summary>
        /// 清空列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public void ClearColumn(int columnIndex)
        {
            CheckColumnIndex(columnIndex);
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                for (int i = 0; i < _rowCount; i++)
                {
                    Set(i, columnIndex, 0d);
                }
            }
            else
            {
                //todo
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
                if (_storageOrder == MatrixMajorOrder.Row)
                {
                    for (int i = rowIndex; i < rowIndex + rCount; i++)
                    {
                        Array.Clear(_elements, i * _colCount + columnIndex, cCount);
                    }
                }
                else
                {
                    //todo
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
        public double[] ToRowMajorArray()
        {
            double[] result = new double[_elements.Length];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                Buffer.BlockCopy(_elements, 0, result, 0, _elements.Length * DoubleSize);
            }
            else
            {
                //todo
            }
            return result;
        }
        public double[] ToColumnMajorArray()
        {
            double[] result = new double[_elements.Length];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                for (int i = 0; i < _colCount; i++)
                {
                    for (int j = 0, offset = i * _colCount; j < _rowCount; j++)
                    {
                        result[offset + j] = _elements[j * _colCount + i];
                    }
                }
            }
            else
            {
                Buffer.BlockCopy(_elements, 0, result, 0, _elements.Length * DoubleSize);
            }
            return result;
        }
        /// <summary>
        /// 转为二维数组
        /// </summary>
        public double[,] To2DArray()
        {
            double[,] result = new double[_rowCount, _colCount];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                Buffer.BlockCopy(_elements, 0, result, 0, result.Length * DoubleSize);
            }
            else
            {
                //todo
            }
            return result;
        }
        /// <summary>
        /// 转为交错数组
        /// </summary>
        public double[][] ToJaggedArray()
        {
            double[][] result = new double[_rowCount][];
            if (_storageOrder == MatrixMajorOrder.Row)
            {
                for (int i = 0; i < _rowCount; i++)
                {
                    result[i] = new double[_colCount];
                    Buffer.BlockCopy(_elements, i * _colCount * DoubleSize, result[i], 0, _colCount * DoubleSize);
                }
            }
            else
            {
                //todo
            }
            return result;
        }
        #endregion
    }
}
