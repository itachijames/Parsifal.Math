namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 解线性方程
    /// </summary>
    public interface ILineraEquationSolver
    {
        /// <summary>
        /// 解方程: <c>AX = B</c>
        /// </summary>
        /// <param name="input">矩阵,<c>B</c></param>
        /// <returns>矩阵,<c>X</c></returns>
        Matrix Solve(Matrix input);
        /// <summary>
        /// 解方程 <c>Ax = b</c>
        /// </summary>
        /// <param name="input">向量,<c>b</c></param>
        /// <returns>向量,<c>x</c></returns>
        Vector Solve(Vector input);
    }
}
