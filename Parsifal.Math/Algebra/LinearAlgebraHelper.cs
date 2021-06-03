using System;

namespace Parsifal.Math.Algebra
{
    internal class LinearAlgebraHelper
    {
        /// <summary>
        /// 获取矩阵元素
        /// </summary>
        public static T GetElement<T>(int rows, int columns, T[] matrix, MatrixMajorOrder majorOrder, int rowIndex, int columnIndex)
        {
            if (majorOrder == MatrixMajorOrder.Row)
            {//行主序
                return matrix[rowIndex * columns + columnIndex];
            }
            else
            {//列主序
                return matrix[columnIndex * rows + rowIndex];
            }
        }
        /// <summary>
        /// 设置矩阵元素
        /// </summary>
        public static void SetElement<T>(int rows, int columns, T[] matrix, MatrixMajorOrder majorOrder, int rowIndex, int columnIndex, T value)
        {
            if (majorOrder == MatrixMajorOrder.Row)
            {//行主序
                matrix[rowIndex * columns + columnIndex] = value;
            }
            else
            {//列主序
                matrix[columnIndex * rows + rowIndex] = value;
            }
        }
        /// <summary>
        /// 获取行
        /// </summary>
        public static void GetRow<T>(int rows, int columns, T[] matrix, MatrixMajorOrder majorOrder, MatrixTranspose transpose, int rowIndex, T[] result)
        {
            if (!(transpose == MatrixTranspose.NotTranspose ^ majorOrder == MatrixMajorOrder.Row))
            {
                Array.Copy(matrix, rowIndex * columns, result, 0, columns);
            }
            else
            {
                for (int i = 0; i < columns; i++)
                {
                    result[i] = matrix[i * rows + rowIndex];
                }
            }
        }
        /// <summary>
        /// 获取列
        /// </summary>
        public static void GetColumn<T>(int rows, int columns, T[] matrix, MatrixMajorOrder majorOrder, MatrixTranspose transpose, int columnIndex, T[] result)
        {
            if (!(transpose == MatrixTranspose.NotTranspose ^ majorOrder == MatrixMajorOrder.Row))
            {
                for (int i = 0; i < rows; i++)
                {
                    result[i] = matrix[i * columns + columnIndex];
                }
            }
            else
            {
                Array.Copy(matrix, columnIndex * rows, result, 0, rows);
            }
        }
    }
}
