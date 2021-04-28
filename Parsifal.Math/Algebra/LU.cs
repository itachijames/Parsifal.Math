using System;

namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// LU分解
    /// </summary>
    /// <remarks>对方阵M，其LU分解为：求得下三角矩阵L、上三角矩阵U，有 M = L*U</remarks>
    public class LU : ILineraEquationSolver
    {
        private readonly Matrix _mat;
        private readonly int[] _pivots;
        private Lazy<Matrix> _lower;
        private Lazy<Matrix> _upper;

        internal LU(Matrix matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            if (!matrix.IsSquare)
                ThrowHelper.ThrowNotSupportedException(ErrorReason.OnlyForSquareMatrix);
            _pivots = new int[matrix.Rows];
            Init();
            _mat = matrix.Clone();
            _lower = new Lazy<Matrix>(() => SetDiagonal(_mat.LowerTriangle()));
            _upper = new Lazy<Matrix>(() => _mat.UpperTriangle());
        }
        private void Init()
        {
            for (int i = 0; i < _pivots.Length; i++)
            {
                _pivots[i] = i;
            }

            var colVec = new double[_mat.Rows];

            for (int i = 0; i < _mat.Rows; i++)
            {

            }
        }
        private Matrix SetDiagonal(Matrix lower)
        {
            //下三角阵对角线化为1
            var rows = lower.Rows;
            for (int i = 0; i < rows; i++)
            {
                lower.Set(i, i, 1d);
            }
            return lower;
        }

        /// <summary>
        /// 行列式
        /// </summary>
        /// <returns></returns>
        public double Determinant()
        {
            double result = 0;
            //todo
            return result;
        }
        /// <summary>
        /// 逆矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            throw new NotImplementedException();
        }

        #region ISolver
        public Matrix Solve(Matrix input)
        {
            if (input == null)
                ThrowHelper.ThrowArgumentNullException(nameof(input));
            if (_mat.Rows != input.Rows)
                ThrowHelper.ThrowDimensionDontMatchException(_mat, input);
            var result = input.Clone();
            //todo 

            return result;
        }

        public Vector Solve(Vector input)
        {
            if (input == null)
                ThrowHelper.ThrowArgumentNullException(nameof(input));

            throw new NotImplementedException();
        }
        #endregion
    }
}
