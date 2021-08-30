using System;

namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 解线性方程
    /// </summary>
    public interface ILineraEquationSolver<T> where T : struct, IEquatable<T>
    {
        /// <summary>
        /// 解方程: <c>AX = B</c>
        /// </summary>
        /// <param name="input">矩阵,<c>B</c></param>
        /// <returns>矩阵,<c>X</c></returns>
        Matrix<T> Solve(Matrix<T> input);
        /// <summary>
        /// 解方程: <c>Ax = b</c>
        /// </summary>
        /// <param name="input">向量,<c>b</c></param>
        /// <returns>向量,<c>x</c></returns>
        Vector<T> Solve(Vector<T> input);
    }
}
