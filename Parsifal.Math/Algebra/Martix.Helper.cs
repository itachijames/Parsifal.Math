namespace Parsifal.Math.Algebra
{
    public partial class Matrix
    {
        /// <summary>
        /// 获取值(不进行边界校验)
        /// </summary>
        /// <param name="index">索引位</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal double Get(int index)
        {
            return _elements[index];
        }
        /// <summary>
        /// 获取值(不进行边界校验)
        /// </summary>
        /// <param name="row">行索引</param>
        /// <param name="column">列索引</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal double Get(int row, int column)
        {
            return _elements[column * _rowCount + row];
        }
        /// <summary>
        /// 设定值(不进行边界校验)
        /// </summary>
        /// <param name="index">索引位</param>
        /// <param name="value">值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int index, double value)
        {
            _elements[index] = value;
        }
        /// <summary>
        /// 设置值(不进行边界校验)
        /// </summary>
        /// <param name="row">行索引</param>
        /// <param name="column">列索引</param>
        /// <param name="value">值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int row, int column, double value)
        {
            _elements[column * _rowCount + row] = value;
        }
        /// <summary>
        /// 根据存储索引获取对应行列索引
        /// </summary>
        internal void GetRowColumnWithIndex(int index, out int row, out int column)
        {
            column = System.Math.DivRem(index, _rowCount, out row);
        }
        private void CheckRowIndex(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _rowCount)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(rowIndex));
        }
        private void CheckColumnIndex(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= _colCount)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(columnIndex));
        }
        private static void CheckValidRowAndColumn(int rows, int columns)
        {
            if (rows < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
            if (columns < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
        }
        private static void CheckSquareMatrix(Matrix matrix)
        {
            if (matrix._rowCount != matrix._colCount)
                ThrowHelper.ThrowNotSupportedException(ErrorReason.OnlyForSquareMatrix);
        }
        private static void CheckSameDimension(Matrix left, Matrix right)
        {
            if (left._rowCount != right._rowCount || left._colCount != right._colCount)
                ThrowHelper.ThrowDimensionDontMatchException(left, right);
        }
        private static void CheckSameRow(Matrix left, Matrix right)
        {
            if (left._rowCount != right._rowCount)
                ThrowHelper.ThrowDimensionDontMatchException(left, right);
        }
        private static void CheckSameColumn(Matrix left, Matrix right)
        {
            if (left._colCount != right._colCount)
                ThrowHelper.ThrowDimensionDontMatchException(left, right);
        }
        private static void CheckMultipliable(Matrix left, Matrix right)
        {
            if (left._colCount != right._rowCount)
                ThrowHelper.ThrowDimensionDontMatchException(left, right);
        }
        private static void CheckMultipliable(Matrix matrix, Vector vector)
        {
            if (matrix._colCount != vector.Dimension)
                ThrowHelper.ThrowDimensionDontMatchException(matrix, vector);
        }
        private static void CheckMultipliable(Vector vector, Matrix matrix)
        {
            if (matrix._rowCount != vector.Dimension)
                ThrowHelper.ThrowDimensionDontMatchException(vector, matrix);
        }
    }
}
