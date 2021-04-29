namespace Parsifal.Math.Algebra
{
    internal class LinearAlgebraHelper
    {
        /// <summary>
        /// 获取矩阵元素
        /// </summary>
        public double GetElement(int rows, int columns, double[] matrix, MatrixMajorOrder majorOrder, int rowIndex, int columnIndex)
        {
            if (majorOrder == MatrixMajorOrder.Row)
            {//行主序
                return matrix[rowIndex * columns + columnIndex];
            }
            else
            {//列主序
                return matrix[columnIndex * rows + rows];
            }
        }
    }
}
