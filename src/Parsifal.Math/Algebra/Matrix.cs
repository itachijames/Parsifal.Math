using System;
using System.Collections;
using System.Collections.Generic;

namespace Parsifal.Math.Algebra
{
    using Math = System.Math;
    /// <summary>
    /// 矩阵
    /// </summary>
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("Matrix ({Rows} × {Columns})")]
    public partial class Matrix : IEnumerable<double>, IEquatable<Matrix>, ICloneable, IFormattable
    {
        #region field
        private const int DoubleSize = sizeof(double);
        /// <summary>
        /// 元素(列主序)
        /// </summary>
        private readonly double[] _elements;
        private readonly int _rowCount;
        private readonly int _colCount;
        #endregion

        #region property
        internal double[] Storage { get => _elements; }
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
            if (rows < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
            if (columns < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
            _rowCount = rows;
            _colCount = columns;
            _elements = new double[rows * columns];
        }
        internal Matrix(int rows, int columns, double[] elements)
        {
            //内部创建不再进行检查
            //if (rows < 1)
            //    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
            //if (columns < 1)
            //    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
            //if (elements is null || elements.Length == 0)
            //    ThrowHelper.ThrowArgumentNullException(nameof(elements));
            if (rows * columns != elements.Length)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(elements));
            _rowCount = rows;
            _colCount = columns;
            _elements = elements;
        }
        #endregion

        #region IEquatable
        public bool Equals(Matrix other) => this == other;
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
            double[] data = new double[_elements.Length];
            Buffer.BlockCopy(_elements, 0, data, 0, _elements.Length * DoubleSize);
            return new Matrix(_rowCount, _colCount, data);
        }
        #endregion

        #region IFormattable
        public string ToString(string format, IFormatProvider formatProvider)
        {
            int tempCol = _colCount - 1;
            var sb = new System.Text.StringBuilder();
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

        #region BCL
        public override bool Equals(object obj) => obj is Matrix mat && Equals(mat);
        public override int GetHashCode() => HashCode.Combine(_rowCount, _colCount, _elements);
        public override string ToString() => ToString(UtilityHelper.DoubleFormat, System.Globalization.CultureInfo.CurrentCulture);
        #endregion

        #region public
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
            double[] data = new double[_rowCount * _colCount];
            for (int i = 0; i < _colCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    data[j * _colCount + i] = Get(j, i);
                }
            }
            return new Matrix(_colCount, _rowCount, data);
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
            //var result = new Matrix(_rowCount, _colCount);
            //for (int i = 0; i < _rowCount; i++)
            //{
            //    for (int j = 0; j <= i && j < _colCount; j++)
            //    {
            //        result.Set(i, j, Get(i, j));
            //    }
            //}
            //return result;
            double[] data = new double[_elements.Length];
            int min = Math.Min(_rowCount, _colCount);
            for (int i = 0; i < min; i++)
            {
                int offset = i * _rowCount + i;
                Buffer.BlockCopy(_elements, offset * DoubleSize,
                    data, offset * DoubleSize,
                    (_rowCount - i) * DoubleSize);
            }
            return new Matrix(_rowCount, _colCount, data);
        }
        /// <summary>
        /// 上三角矩阵
        /// </summary>
        public Matrix UpperTriangle()
        {
            //var result = new Matrix(_rowCount, _colCount);
            //for (int i = 0; i < _rowCount; i++)
            //{
            //    for (int j = i; j < _colCount; j++)
            //    {
            //        result.Set(i, j, Get(i, j));
            //    }
            //}
            //return result;
            double[] data = new double[_elements.Length];
            int min = Math.Min(_rowCount, _colCount);
            for (int i = 0; i < min; i++)
            {
                Buffer.BlockCopy(_elements, i * _rowCount * DoubleSize,
                    data, i * _rowCount * DoubleSize,
                    (i + 1) * DoubleSize);
            }
            return new Matrix(_rowCount, _colCount, data);
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
            for (int i = 0; i < _colCount; i++)
            {
                result[i] = Get(rowIndex, i);
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
            Buffer.BlockCopy(_elements, columnIndex * _rowCount * DoubleSize, result, 0, _rowCount * DoubleSize);
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
            for (int i = 0; i < cCount; i++)
            {
                Buffer.BlockCopy(_elements, ((columnIndex + i) * _rowCount + rowIndex) * DoubleSize,
                    data, i * rCount * DoubleSize,
                    rCount * DoubleSize);
            }
            return new Matrix(rCount, cCount, data);
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
            for (int i = 0; i < _colCount; i++)
            {
                Set(rowIndex, i, row[i]);
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
            Buffer.BlockCopy(column, 0,
                _elements, columnIndex * _rowCount * DoubleSize,
                _rowCount * DoubleSize);
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
            for (int i = 0; i < subMatrix._colCount; i++)
            {
                Buffer.BlockCopy(subMatrix._elements, i * subMatrix._rowCount * DoubleSize,
                    _elements, ((columnIndex + i) * _rowCount + rowIndex) * DoubleSize,
                    subMatrix._rowCount * DoubleSize);
            }
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
            for (int i = 0; i < _colCount; i++)
            {
                Buffer.BlockCopy(_elements, i * _rowCount * DoubleSize,
                    data, i * rows * DoubleSize,
                    _rowCount * DoubleSize);
                Buffer.BlockCopy(matrix._elements, i * matrix._rowCount * DoubleSize,
                    data, ((i * rows) + _rowCount) * DoubleSize,
                    matrix._rowCount * DoubleSize);
            }
            return new Matrix(rows, _colCount, data);
        }
        /// <summary>
        /// 添加到右方
        /// </summary>
        /// <param name="matrix">待添加矩阵</param>
        public Matrix ConcatenateRight(Matrix matrix)
        {
            CheckSameRow(this, matrix);
            int cols = _colCount + matrix._colCount;
            double[] data = new double[_rowCount * cols];
            Buffer.BlockCopy(_elements, 0, data, 0, _elements.Length * DoubleSize);
            Buffer.BlockCopy(matrix._elements, 0, data, _elements.Length * DoubleSize, matrix._elements.Length * DoubleSize);
            return new Matrix(_rowCount, cols, data);
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
            for (int i = 0; i < _colCount; i++)
            {
                Set(rowIndex, i, 0d);
            }
        }
        /// <summary>
        /// 清空列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public void ClearColumn(int columnIndex)
        {
            CheckColumnIndex(columnIndex);
            Array.Clear(_elements, columnIndex * _rowCount, _rowCount);
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
                for (int i = columnIndex; i < columnIndex + cCount; i++)
                {
                    Array.Clear(_elements, i * _rowCount + rowIndex, rCount);
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
        /// 转为行主序的一维数组
        /// </summary>
        public double[] ToRowMajorArray()
        {
            double[] result = new double[_elements.Length];
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0, temp = i * _colCount; j < _colCount; j++)
                {
                    result[temp + j] = Get(i, j);
                }
            }
            return result;
        }
        /// <summary>
        /// 转为列主序的一维数组
        /// </summary>
        public double[] ToColumnMajorArray()
        {
            double[] result = new double[_elements.Length];
            Buffer.BlockCopy(_elements, 0, result, 0, _elements.Length * DoubleSize);
            return result;
        }
        /// <summary>
        /// 转为二维数组
        /// </summary>
        public double[,] To2DArray()
        {
            double[,] result = new double[_rowCount, _colCount];
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _colCount; j++)
                {
                    result[i, j] = Get(i, j);
                }
            }
            return result;
        }
        /// <summary>
        /// 转为行交错数组
        /// </summary>
        public double[][] ToRowJaggedArray()
        {
            double[][] result = new double[_rowCount][];
            for (int i = 0; i < _rowCount; i++)
            {
                double[] rows = new double[_colCount];
                for (int j = 0; j < _colCount; j++)
                {
                    rows[j] = Get(i, j);
                }
                result[i] = rows;
            }
            return result;
        }
        /// <summary>
        /// 转为列交错数组
        /// </summary>
        public double[][] ToColumnJaggedArray()
        {
            double[][] result = new double[_colCount][];
            for (int i = 0; i < _colCount; i++)
            {
                double[] cols = new double[_rowCount];
                Buffer.BlockCopy(_elements, i * _rowCount * DoubleSize,
                    cols, 0,
                    _rowCount * DoubleSize);
                result[i] = cols;
            }
            return result;
        }
        #endregion
    }
}
