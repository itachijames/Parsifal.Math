namespace Parsifal.Math
{
    public interface ILogicProvider
    {
        LogicProviderType Provider { get; }

        #region LinearAlgebra
        /// <summary>
        /// 数组缩放 <paramref name="result"/> = <paramref name="scalar"/> * <paramref name="source"/>
        /// </summary>
        /// <param name="scalar">缩放比例</param>
        /// <param name="source">数组</param>
        /// <param name="result">结果</param>
        void ScalarArray(double scalar, double[] source, double[] result);
        /// <summary>
        /// 数组相加 <paramref name="result"/> = <paramref name="x"/> + <paramref name="y"/>
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="result">结果</param>
        void AddArray(double[] x, double[] y, double[] result);
        /// <summary>
        /// 数组相减 <paramref name="result"/> = <paramref name="x"/> - <paramref name="y"/>
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="result">结果</param>
        void SubtractArray(double[] x, double[] y, double[] result);
        /// <summary>
        /// 向量点乘 
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="result">点积</param>
        void VectorDotProduct(double[] x, double[] y, double result);
        /// <summary>
        /// 矩阵乘法 <paramref name="result"/> = <paramref name="x"/> × <paramref name="y"/>
        /// </summary>
        /// <param name="rowsX">X行数</param>
        /// <param name="columnsX">X列数</param>
        /// <param name="x">矩阵X</param>
        /// <param name="rowsY">Y行数</param>
        /// <param name="columnsY">Y列数</param>
        /// <param name="y">矩阵Y</param>
        /// <param name="result">结果矩阵</param>
        void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result);

        #endregion
    }
}
