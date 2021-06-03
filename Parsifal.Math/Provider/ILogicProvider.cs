using Parsifal.Math.Algebra;

namespace Parsifal.Math
{
    public interface ILogicProvider
    {
        LogicProviderType Provider { get; }

        #region Versatile
        /// <summary>
        /// 数组数乘 <paramref name="result"/> = <paramref name="scalar"/> * <paramref name="x"/>
        /// </summary>
        /// <param name="scalar">乘数</param>
        /// <param name="x">数组</param>
        /// <param name="result">结果</param>
        void ArrayMultiply(double scalar, double[] x, double[] result);
        /// <summary>
        /// 数组加法 <paramref name="result"/> = <paramref name="scalar"/> + <paramref name="x"/>
        /// </summary>
        /// <param name="scalar">加数</param>
        /// <param name="x">数组</param>
        /// <param name="result">结果</param>
        void ArrayAddScalar(double scalar, double[] x, double[] result);
        /// <summary>
        /// 数组相加 <paramref name="result"/> = <paramref name="x"/> + <paramref name="y"/>
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="result">结果</param>
        void ArrayAdd(double[] x, double[] y, double[] result);
        /// <summary>
        /// 数组相减 <paramref name="result"/> = <paramref name="x"/> - <paramref name="y"/>
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="result">结果</param>
        void ArraySubtract(double[] x, double[] y, double[] result);
        #endregion

        #region LinearAlgebra
        /// <summary>
        /// 向量点乘
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns></returns>
        double VectorDotProduct(double[] x, double[] y);
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
        /// <summary>
        /// 矩阵乘法 <paramref name="result"/> = <paramref name="alpha"/>*<paramref name="x"/> × <paramref name="y"/> + <paramref name="beta"/>*<paramref name="result"/>
        /// </summary>
        /// <param name="alpha">alpha</param>
        /// <param name="rowsX">X行数</param>
        /// <param name="columnsX">X列数</param>
        /// <param name="x">矩阵X</param>
        /// <param name="transposeX">X转置</param>
        /// <param name="rowsY">Y行数</param>
        /// <param name="columnsY">Y列数</param>
        /// <param name="y">矩阵Y</param>
        /// <param name="transposeY">Y转置</param>
        /// <param name="beta">beta</param>
        /// <param name="result">结果矩阵</param>
        public void MatrixMultiply(double alpha, int rowsX, int columnsX, double[] x, MatrixTranspose transposeX, int rowsY, int columnsY, double[] y, MatrixTranspose transposeY, double beta, double[] result);
        #endregion
    }
}
