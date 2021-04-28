namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 线性代数算法实现
    /// </summary>
    internal interface ILinearAlgebraProvider
    {
        /// <summary>
        /// 矩阵乘法 result = X × Y
        /// </summary>
        /// <param name="rowsX">X行数</param>
        /// <param name="columnsX">X列数</param>
        /// <param name="x">矩阵X</param>
        /// <param name="rowsY">Y行数</param>
        /// <param name="columnsY">Y列数</param>
        /// <param name="y">矩阵Y</param>
        /// <param name="result">结果矩阵</param>
        public void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result);
    }
}
