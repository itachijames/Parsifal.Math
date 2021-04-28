namespace Parsifal.Math.Algebra
{
    public partial class Matrix
    {
        /// <summary>
        /// 获取值(不进行边界校验)
        /// </summary>
        /// <param name="index">索引位</param>
        /// <returns></returns>
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
        internal double Get(int row, int column)
        {
            return _elements[row * _colCount + column];
        }
        /// <summary>
        /// 设定值(不进行边界校验)
        /// </summary>
        /// <param name="index">索引位</param>
        /// <param name="value">值</param>
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
        internal void Set(int row, int column, double value)
        {
            _elements[row * _colCount + column] = value;
        }
        /// <summary>是否不应使用并行</summary>
        /// <remarks>用于指示在使用<b>原生算法</b>时是否使用并行运算</remarks>
        /// <returns>不应使用返回true;酌情可使用并行时返回false</returns>
        private bool ShouldNotUseParallel()
        {//过小阶的矩阵采用并行会导致不必要的开销，而失去计算优势
            return System.Math.Max(_rowCount, _colCount) < 32;
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
            if (!matrix.IsSquare)
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
