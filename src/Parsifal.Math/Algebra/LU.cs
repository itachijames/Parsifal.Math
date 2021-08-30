using System;

namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// LU分解
    /// </summary>
    /// <remarks>对方阵M，其LU分解为：求得下三角矩阵L、上三角矩阵U，有 M = L*U</remarks>
    public class LU<T> : ILineraEquationSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        private readonly Matrix<T> _mat;
        private readonly int[] _pivots;
        private Lazy<Matrix<T>> _lower;
        private Lazy<Matrix<T>> _upper;

        internal LU(Matrix<T> matrix)
        {
        }

        /// <summary>
        /// 行列式
        /// </summary>
        /// <returns></returns>
        public T Determinant()
        {
            return default;
        }
        /// <summary>
        /// 逆矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix<T> Inverse()
        {
            throw new NotImplementedException();
        }

        #region ILineraEquationSolver
        public Matrix<T> Solve(Matrix<T> input)
        {
            if (input is null)
                ThrowHelper.ThrowArgumentNullException(nameof(input));
            if (_mat.Rows != input.Rows)
                ThrowHelper.ThrowDimensionDontMatchException(_mat, input);

            throw new NotImplementedException();
        }

        public Vector<T> Solve(Vector<T> input)
        {
            if (input == null)
                ThrowHelper.ThrowArgumentNullException(nameof(input));

            throw new NotImplementedException();
        }
        #endregion
    }
}
