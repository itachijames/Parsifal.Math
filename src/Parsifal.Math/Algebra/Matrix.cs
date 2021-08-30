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
    public partial class Matrix<T> : AlgebraGenricBase<T>, IEnumerable<T>, IEquatable<Matrix<T>>, ICloneable, IFormattable
        where T : struct, IEquatable<T>, IFormattable
    {
        #region field
        /// <summary>
        /// 元素(列主序)
        /// </summary>
        private readonly T[] _elements;
        private readonly int _rowCount;
        private readonly int _colCount;
        #endregion

        #region property
        internal T[] Storage { get => _elements; }

        public T this[int rowIndex, int columnIndex]
        {
            get
            {
                CheckRowIndex(rowIndex);
                CheckColumnIndex(columnIndex);
                return Get(rowIndex, columnIndex);
            }
            set
            {
                CheckRowIndex(rowIndex);
                CheckColumnIndex(columnIndex);
                Set(rowIndex, columnIndex, value);
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
        public Matrix(int rows, int columns)
        {
            CheckValidRowAndColumn(rows, columns);
            _rowCount = rows;
            _colCount = columns;
            _elements = new T[rows * columns];
        }
        internal Matrix(int rows, int columns, T[] elements)
        {
            if (rows * columns != elements.Length)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(elements));
            _rowCount = rows;
            _colCount = columns;
            _elements = elements;
        }
        #endregion

        #region IEquatable
        public bool Equals(Matrix<T> other)
        {
            if (other is null)
                return false;
            if (_rowCount != other._rowCount || _colCount != other._colCount)
                return false;
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!Get(i).Equals(other.Get(i)))
                    return false;
            }
            return true;
        }
        #endregion

        #region IEnumerable
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                yield return Get(i);
            }
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
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
        public Matrix<T> Clone()
        {
            T[] data = new T[_elements.Length];
            Array.Copy(_elements, 0, data, 0, _elements.Length);
            return new Matrix<T>(_rowCount, _colCount, data);
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
        public override bool Equals(object obj) => obj is Matrix<T> matrix && Equals(matrix);
        public override int GetHashCode() => HashCode.Combine(_rowCount, _colCount, _elements);
        public override string ToString() => ToString(UtilityHelper.DigitalFormat, System.Globalization.CultureInfo.CurrentCulture);
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
                    if (!Get(i, j).Equals(Get(j, i)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public LU<T> LU()
        {
            return new LU<T>(this);
        }
        public QR<T> QR()
        {
            throw new NotImplementedException();
        }
        public Svd<T> Svd()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 行列式
        /// </summary>
        public T Determinant()
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
        public T Cofactor(int rowIndex, int columnIndex)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 条件数
        /// </summary>
        public T ConditionNumber()
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
        public T Trace()
        {
            //CheckSquareMatrix(this);
            //T result = default;
            //for (int i = 0; i < _rowCount; i++)
            //{
            //    result += Get(i, i);
            //}
            //return result;
            throw new NotImplementedException();
        }
        /// <summary>
        /// 求逆矩阵
        /// </summary>
        /// <returns>逆矩阵</returns>
        public Matrix<T> Inverse()
        {
            CheckSquareMatrix(this);
            return LU().Inverse();
        }
        /// <summary>
        /// 伪逆矩阵
        /// </summary>
        public Matrix<T> PseudoInverse()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取转置矩阵
        /// </summary>
        /// <returns>转置矩阵</returns>
        public Matrix<T> Transpose()
        {
            T[] data = new T[_rowCount * _colCount];
            for (int i = 0; i < _colCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    data[j * _colCount + i] = Get(j, i);
                }
            }
            return new Matrix<T>(_colCount, _rowCount, data);
        }
        /// <summary>
        /// 幂矩阵
        /// </summary>
        /// <param name="exponent">幂次</param>
        /// <returns></returns>
        public Matrix<T> Power(int exponent)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 下三角矩阵
        /// </summary>
        public Matrix<T> LowerTriangle()
        {
            T[] data = new T[_elements.Length];
            int min = Math.Min(_rowCount, _colCount);
            for (int i = 0; i < min; i++)
            {
                int offset = i * _rowCount + i;
                Array.Copy(_elements, offset, data, offset, _rowCount - 1);
            }
            return new Matrix<T>(_rowCount, _colCount, data);
        }
        /// <summary>
        /// 上三角矩阵
        /// </summary>
        public Matrix<T> UpperTriangle()
        {
            T[] data = new T[_elements.Length];
            int min = Math.Min(_rowCount, _colCount);
            for (int i = 0; i < min; i++)
            {
                Array.Copy(_elements, i * _rowCount, data, _rowCount, i + 1);
            }
            return new Matrix<T>(_rowCount, _colCount, data);
        }
        /// <summary>
        /// 核
        /// </summary>
        public T[] Kernel()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 象
        /// </summary>
        public T[] Image()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Norm
        /// <summary>
        /// 1范数
        /// </summary>
        /// <remarks>列向量绝对值之和的最大值</remarks>
        public T OneNorm()
        {
            //T norm;
            //for (int i = 0; i < _colCount; i++)
            //{
            //    T temp = default;
            //    for (int j = 0; j < _rowCount; j++)
            //    {
            //        temp += Math.Abs(Get(j, i));
            //    }
            //    norm = Math.Max(temp, norm);
            //}
            //return norm;
            throw new NotImplementedException();
        }
        /// <summary>
        /// 2范数
        /// </summary>
        /// <remarks>(AT)*A 的最大特征值开方 √(λmax)</remarks>
        public T TwoNorm()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 无穷范数
        /// </summary>
        /// <remarks>行向量绝对值之和的最大值</remarks>
        public T InfinityNorm()
        {
            //double norm = 0d;
            //for (int i = 0; i < _rowCount; i++)
            //{
            //    double temp = 0d;
            //    for (int j = 0; j < _colCount; j++)
            //    {
            //        temp += Math.Abs(Get(i, j));
            //    }
            //    norm = Math.Max(temp, norm);
            //}
            //return norm;
            throw new NotImplementedException();
        }
        /// <summary>
        /// F范数
        /// </summary>
        /// <remarks>所有元素平方和的开方</remarks>
        public T FrobeniusNorm()
        {
            //T temp;
            //for (int i = 0; i < _elements.Length; i++)
            //{
            //    temp += Math.Pow(Get(i), 2);
            //}
            //return Math.Sqrt(temp);
            throw new NotImplementedException();
        }
        /// <summary>
        /// 核范数
        /// </summary>
        /// <remarks>奇异值之和</remarks>
        public T NuclearNorm()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region MatrixOperation
        /// <summary>
        /// 获取行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public IEnumerable<T> GetRow(int rowIndex)
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
        public T[] GetRowArray(int rowIndex)
        {
            CheckRowIndex(rowIndex);
            T[] result = new T[_colCount];
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
        public Vector<T> GetRowVector(int rowIndex)
        {
            return GetRowArray(rowIndex);
        }
        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public IEnumerable<T> GetColumn(int columnIndex)
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
        public T[] GetColumnArray(int columnIndex)
        {
            CheckColumnIndex(columnIndex);
            T[] result = new T[_rowCount];
            Array.Copy(_elements, columnIndex * _rowCount, result, 0, _rowCount);
            return result;
        }
        /// <summary>
        /// 获取列向量
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <returns>列向量</returns>
        public Vector<T> GetColumnVector(int columnIndex)
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
        public Matrix<T> GetSubMatrix(int rowIndex, int rCount, int columnIndex, int cCount)
        {
            if (rCount < 1 || cCount < 1)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.NotPositiveParameter);
            if (rowIndex < 0 || rowIndex + rCount > _rowCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowIndex));
            if (columnIndex < 0 || columnIndex + cCount > _colCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnIndex));
            T[] data = new T[rCount * cCount];
            for (int i = 0; i < cCount; i++)
            {
                Array.Copy(_elements, (columnIndex + i) * _rowCount + rowIndex, data, i * rCount, rCount);
            }
            return new Matrix<T>(rCount, cCount, data);
        }
        /// <summary>
        /// 设置行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="row">行元素</param>
        public void SetRow(int rowIndex, T[] row)
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
        public void SetRow(int rowIndex, Vector<T> row)
        {
            SetRow(rowIndex, row.Storage);
        }
        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="column">列元素</param>
        public void SetColumn(int columnIndex, T[] column)
        {
            CheckColumnIndex(columnIndex);
            if (column is null)
                ThrowHelper.ThrowArgumentNullException(nameof(column));
            if (column.Length != _rowCount)
                ThrowHelper.ThrowDimensionDontMatchException(this, column);
            Array.Copy(column, 0, _elements, columnIndex * _rowCount, _rowCount);
        }
        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="column">列向量</param>
        public void SetColumn(int columnIndex, Vector<T> column)
        {
            SetColumn(columnIndex, column.Storage);
        }
        /// <summary>
        /// 设置子矩阵
        /// </summary>
        /// <param name="rowIndex">起始行索引</param>
        /// <param name="columnIndex">起始列索引</param>
        /// <param name="subMatrix">子矩阵</param>
        public void SetSubMatrix(int rowIndex, int columnIndex, Matrix<T> subMatrix)
        {
            if (subMatrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(subMatrix));
            if (rowIndex < 0 || rowIndex + subMatrix._rowCount > _rowCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowIndex));
            if (columnIndex < 0 || columnIndex + subMatrix._colCount > _colCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnIndex));
            for (int i = 0; i < subMatrix._colCount; i++)
            {
                Array.Copy(subMatrix._elements, i * subMatrix._rowCount,
                    _elements, (columnIndex + i) * _rowCount + rowIndex,
                    subMatrix._rowCount);
            }
        }
        /// <summary>
        /// 添加到下方
        /// </summary>
        /// <param name="matrix">待添加矩阵</param>
        public Matrix<T> ConcatenateBelow(Matrix<T> matrix)
        {
            CheckSameColumn(this, matrix);
            int rows = _rowCount + matrix._rowCount;
            T[] data = new T[rows * _colCount];
            for (int i = 0; i < _colCount; i++)
            {
                Array.Copy(_elements, i * _rowCount, data, i * rows, _rowCount);
                Array.Copy(matrix._elements, i * matrix._rowCount, data, (i * rows) + _rowCount, matrix._rowCount);
            }
            return new Matrix<T>(rows, _colCount, data);
        }
        /// <summary>
        /// 添加到右方
        /// </summary>
        /// <param name="matrix">待添加矩阵</param>
        public Matrix<T> ConcatenateRight(Matrix<T> matrix)
        {
            CheckSameRow(this, matrix);
            int cols = _colCount + matrix._colCount;
            T[] data = new T[_rowCount * cols];
            Array.Copy(_elements, 0, data, 0, _elements.Length);
            Array.Copy(matrix._elements, 0, data, _elements.Length, matrix._elements.Length);
            return new Matrix<T>(_rowCount, cols, data);
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
                Set(rowIndex, i, Zero);
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
        /// <param name="zeroPredicate">为0判定</param>
        public void CoerceZero(Predicate<T> zeroPredicate)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (zeroPredicate(Get(i)))
                {
                    Set(i, Zero);
                }
            }
        }
        /// <summary>
        /// 转为行主序的一维数组
        /// </summary>
        public T[] ToRowMajorArray()
        {
            T[] result = new T[_elements.Length];
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
        public T[] ToColumnMajorArray()
        {
            T[] result = new T[_elements.Length];
            Array.Copy(_elements, 0, result, 0, _elements.Length);
            return result;
        }
        /// <summary>
        /// 转为二维数组
        /// </summary>
        public T[,] To2DArray()
        {
            T[,] result = new T[_rowCount, _colCount];
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
        public T[][] ToRowJaggedArray()
        {
            T[][] result = new T[_rowCount][];
            for (int i = 0; i < _rowCount; i++)
            {
                T[] rows = new T[_colCount];
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
        public T[][] ToColumnJaggedArray()
        {
            T[][] result = new T[_colCount][];
            for (int i = 0; i < _colCount; i++)
            {
                T[] cols = new T[_rowCount];
                Array.Copy(_elements, i * _rowCount, cols, 0, _rowCount);
                result[i] = cols;
            }
            return result;
        }
        #endregion
    }
}
