using Parsifal.Math.Algebra;

namespace Parsifal.Math
{
    public enum LinearAlgebraProviderType

    {
        /// <summary>
        /// pure c#
        /// </summary>
        Native,
        /// <summary>
        /// Intel MKL
        /// </summary>
        MKL,
        /// <summary>
        /// Nvidia CUDA
        /// </summary>
        CUDA,
        //more to add
    }
    public interface ILinearAlgebraLogicGeneric<T> where T : struct
    {
        #region Versatile
        /// <summary>
        /// 数组数乘 <paramref name="result"/> = <paramref name="scalar"/> * <paramref name="source"/>
        /// </summary>
        void ArrayMultiply(T scalar, T[] source, T[] result);
        /// <summary>
        /// 数组加法 <paramref name="result"/> = <paramref name="scalar"/> + <paramref name="source"/>
        /// </summary>
        void ArrayAddScalar(T scalar, T[] source, T[] result);
        /// <summary>
        /// 数组相加 <paramref name="result"/> = <paramref name="first"/> + <paramref name="second"/>
        /// </summary>
        void ArrayAdd(T[] first, T[] second, T[] result);
        /// <summary>
        /// 数组相减 <paramref name="result"/> = <paramref name="first"/> - <paramref name="second"/>
        /// </summary>
        void ArraySubtract(T[] first, T[] second, T[] result);
        #endregion

        #region LinearAlgebra
        /// <summary>
        /// 向量点乘
        /// </summary>
        T VectorDotProduct(T[] first, T[] second);
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
        void MatrixMultiply(int rowsX, int columnsX, T[] x, int rowsY, int columnsY, T[] y, T[] result);
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
        public void MatrixMultiply(T alpha, int rowsX, int columnsX, T[] x, MatrixTranspose transposeX, int rowsY, int columnsY, T[] y, MatrixTranspose transposeY, T beta, T[] result);
        #endregion
    }

    public interface LinearAlgebraProvider : ILinearAlgebraLogicGeneric<float>, ILinearAlgebraLogicGeneric<double>
    {
        /// <summary>
        /// 类型
        /// </summary>
        LinearAlgebraProviderType ProviderType { get; }

        ILinearAlgebraLogicGeneric<T> GetLineraAlgebraLogic<T>() where T : struct
        {
            return null;
        }


    }
}
