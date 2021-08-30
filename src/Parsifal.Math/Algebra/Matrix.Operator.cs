namespace Parsifal.Math.Algebra
{
    partial class Matrix<T>
    {
        #region static
        public static Matrix<T> Negate(Matrix<T> matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            T[] data = new T[matrix._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayMultiply(-1, matrix._elements, data);
            return new Matrix<T>(matrix._rowCount, matrix._colCount, data);
        }
        public static Matrix<T> Add(Matrix<T> matrix, T scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            T[] data = new T[matrix._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayAddScalar(scalar, matrix._elements, data);
            return new Matrix<T>(matrix._rowCount, matrix._colCount, data);
        }
        public static Matrix<T> Add(T scalar, Matrix<T> matrix)
        {
            return Matrix<T>.Add(matrix, scalar);
        }
        public static Matrix<T> Add(Matrix<T> left, Matrix<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            T[] data = new T[left._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayAdd(left._elements, right._elements, data);
            return new Matrix<T>(left._rowCount, left._colCount, data);
        }
        public static Matrix<T> Subtract(Matrix<T> matrix, T scalar)
        {
            return Matrix<T>.Add(matrix, -1 * scalar);
        }
        public static Matrix<T> Subtract(T scalar, Matrix<T> matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            T[] temp = new T[matrix._elements.Length];
            T[] data = new T[matrix._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayMultiply(-1, matrix._elements, temp);
            LogicControl.LinearAlgebraProvider.ArrayAddScalar(scalar, temp, data);
            return new Matrix<T>(matrix._rowCount, matrix._colCount, data);
        }
        public static Matrix<T> Subtract(Matrix<T> left, Matrix<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            T[] data = new T[left._elements.Length];
            LogicControl.LinearAlgebraProvider.ArraySubtract(left._elements, right._elements, data);
            return new Matrix<T>(left._rowCount, left._colCount, data);
        }
        public static Matrix<T> Multiply(Matrix<T> matrix, T scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            T[] data = new T[matrix._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayMultiply(scalar, matrix._elements, data);
            return new Matrix<T>(matrix._rowCount, matrix._colCount, data);
        }
        public static Matrix<T> Multiply(T scalar, Matrix<T> matrix)
        {
            return Matrix<T>.Multiply(matrix, scalar);
        }
        public static Matrix<T> Multiply(Matrix<T> left, Matrix<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckMultipliable(left, right);
            T[] data = new T[left._rowCount * right._colCount];
            LogicControl.LinearAlgebraProvider.MatrixMultiply(left._rowCount, left._colCount, left._elements,
                right._rowCount, right._colCount, right._elements,
                data);
            return new Matrix<T>(left._rowCount, right._colCount, data);
        }
        public static Vector<T> Multiply(Matrix<T> matrix, Vector<T> vector)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckMultipliable(matrix, vector);
            T[] data = new T[matrix._rowCount];
            LogicControl.LinearAlgebraProvider.MatrixMultiply(matrix._rowCount, matrix._colCount, matrix._elements,
                vector.Count, 1, vector.Storage,
                data);
            return new Vector<T>(data);
        }
        public static Vector<T> Multiply(Vector<T> vector, Matrix<T> matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckMultipliable(vector, matrix);
            T[] data = new T[matrix._colCount];
            LogicControl.LinearAlgebraProvider.MatrixMultiply(1, vector.Count, vector.Storage,
                matrix._rowCount, matrix._colCount, matrix._elements,
                data);
            return new Vector<T>(data);
        }
        public static Matrix<T> Divide(Matrix<T> matrix, T scalar)
        {
            if (scalar.Equals(Zero))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.ZeroParameter, nameof(scalar));
            return Matrix<T>.Multiply(matrix, 1 / scalar);
        }
        /// <summary>
        /// <paramref name="left"/>乘以<paramref name="right"/>的转置
        /// </summary>
        public static Matrix<T> MultiplyTranspose(Matrix<T> left, Matrix<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameColumn(left, right);
            T[] data = new T[left._rowCount * right._rowCount];
            LogicControl.LinearAlgebraProvider.MatrixMultiply(1.0,
                left._rowCount, left._colCount, left._elements, MatrixTranspose.NotTranspose,
                right._rowCount, right._colCount, right._elements, MatrixTranspose.Transpose,
                0, data);
            return new Matrix<T>(left._rowCount, right._rowCount, data);
        }
        /// <summary>
        /// <paramref name="left"/>的转置乘以<paramref name="right"/>
        /// </summary>
        public static Matrix<T> TransposeMultiply(Matrix<T> left, Matrix<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameRow(left, right);
            T[] data = new T[left._colCount * right._colCount];
            LogicControl.LinearAlgebraProvider.MatrixMultiply(1.0,
                left._rowCount, left._colCount, left._elements, MatrixTranspose.Transpose,
                right._rowCount, right._colCount, right._elements, MatrixTranspose.NotTranspose,
                0, data);
            return new Matrix<T>(left._colCount, right._colCount, data);
        }
        #endregion

        #region instance
        public Matrix<T> Negate()
        {
            return Matrix<T>.Negate(this);
        }
        public Matrix<T> Add(T scalar)
        {
            return Matrix<T>.Add(this, scalar);
        }
        public Matrix<T> Add(Matrix<T> other)
        {
            return Matrix<T>.Add(this, other);
        }
        public Matrix<T> Subtract(T scalar)
        {
            return Matrix<T>.Subtract(this, scalar);
        }
        public Matrix<T> Subtract(Matrix<T> other)
        {
            return Matrix<T>.Subtract(this, other);
        }
        public Matrix<T> Multiply(T scalar)
        {
            return Matrix<T>.Multiply(this, scalar);
        }
        public Matrix<T> Multiply(Matrix<T> matrix)
        {
            return Matrix<T>.Multiply(this, matrix);
        }
        public Vector<T> Multiply(Vector<T> vector)
        {
            return Matrix<T>.Multiply(this, vector);
        }
        public Vector<T> LeftMultiply(Vector<T> vector)
        {
            return Matrix<T>.Multiply(vector, this);
        }
        public Matrix<T> Divide(T scalar)
        {
            return Matrix<T>.Divide(this, scalar);
        }
        public Matrix<T> TransposeThenMultiply(Matrix<T> matrix)
        {
            return Matrix<T>.TransposeMultiply(this, matrix);
        }
        public Matrix<T> MultiplyTransposeOf(Matrix<T> matrix)
        {
            return Matrix<T>.MultiplyTranspose(this, matrix);
        }
        #endregion

        #region operator
        public static explicit operator Matrix<T>(T[,] element)
        {//显式转换
            return CreateByArray(element);
        }
        public static bool operator ==(Matrix<T> left, Matrix<T> right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }
        public static bool operator !=(Matrix<T> left, Matrix<T> right)
        {
            return !(left == right);
        }
        public static Matrix<T> operator +(Matrix<T> matrix, T scalar)
        {
            return Matrix<T>.Add(matrix, scalar);
        }
        public static Matrix<T> operator +(T scalar, Matrix<T> matrix)
        {
            return Matrix<T>.Add(scalar, matrix);
        }
        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
        {
            return Matrix<T>.Add(left, right);
        }
        public static Matrix<T> operator -(Matrix<T> matrix)
        {
            return Matrix<T>.Negate(matrix);
        }
        public static Matrix<T> operator -(Matrix<T> matrix, T scalar)
        {
            return Matrix<T>.Subtract(matrix, scalar);
        }
        public static Matrix<T> operator -(T scalar, Matrix<T> matrix)
        {
            return Matrix<T>.Subtract(scalar, matrix);
        }
        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
        {
            return Matrix<T>.Subtract(left, right);
        }
        public static Matrix<T> operator *(Matrix<T> matrix, T scalar)
        {
            return Matrix<T>.Multiply(matrix, scalar);
        }
        public static Matrix<T> operator *(T scalar, Matrix<T> matrix)
        {
            return Matrix<T>.Multiply(scalar, matrix);
        }
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
        {
            return Matrix<T>.Multiply(left, right);
        }
        public static Vector<T> operator *(Matrix<T> matrix, Vector<T> vector)
        {
            return Matrix<T>.Multiply(matrix, vector);
        }
        public static Vector<T> operator *(Vector<T> vector, Matrix<T> matrix)
        {
            return Matrix<T>.Multiply(vector, matrix);
        }
        public static Matrix<T> operator /(Matrix<T> matrix, T scalar)
        {
            return Matrix<T>.Divide(matrix, scalar);
        }
        #endregion
    }
}
