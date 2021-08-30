using System;

namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// QR分解
    /// </summary>
    public class QR<T> : ILineraEquationSolver<T>
        where T : struct, IEquatable<T>
    {
        #region ILineraEquationSolver
        public Matrix<T> Solve(Matrix<T> input)
        {
            throw new NotImplementedException();
        }

        public Vector<T> Solve(Vector<T> input)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
