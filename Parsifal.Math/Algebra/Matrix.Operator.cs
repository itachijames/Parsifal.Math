namespace Parsifal.Math.Algebra
{
    public partial class Matrix
    {
        public static Matrix Negate(Matrix matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            var result = new Matrix(matrix._rowCount, matrix._colCount, false);
            for (int i = 0; i < result._elements.Length; i++)
            {
                result.Set(i, -1d * matrix.Get(i));
            }
            return result;
        }
        public static Matrix Add(Matrix matrix, double scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            var result = new Matrix(matrix._rowCount, matrix._colCount, false);
            for (int i = 0; i < result._elements.Length; i++)
            {
                result.Set(i, scalar + matrix.Get(i));
            }
            return result;
        }
        public static Matrix Add(double scalar, Matrix matrix)
        {
            return Matrix.Add(matrix, scalar);
        }
        public static Matrix Add(Matrix left, Matrix right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            var result = new Matrix(left._rowCount, left._colCount, false);
            for (int i = 0; i < result._elements.Length; i++)
            {
                result.Set(i, left.Get(i) + right.Get(i));
            }
            return result;
        }
        public static Matrix Subtract(Matrix matrix, double scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            var result = new Matrix(matrix._rowCount, matrix._colCount, false);
            for (int i = 0; i < result._elements.Length; i++)
            {
                result.Set(i, matrix.Get(i) - scalar);
            }
            return result;
        }
        public static Matrix Subtract(double scalar, Matrix matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            var result = new Matrix(matrix._rowCount, matrix._colCount, false);
            for (int i = 0; i < result._elements.Length; i++)
            {
                result.Set(i, scalar - matrix.Get(i));
            }
            return result;
        }
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            var result = new Matrix(left._rowCount, left._colCount, false);
            for (int i = 0; i < result._elements.Length; i++)
            {
                result.Set(i, left.Get(i) - right.Get(i));
            }
            return result;
        }
        public static Matrix Multiply(Matrix matrix, double scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            var result = new Matrix(matrix._rowCount, matrix._colCount, false);
            for (int i = 0; i < result._elements.Length; i++)
            {
                result.Set(i, scalar * matrix.Get(i));
            }
            return result;
        }
        public static Matrix Multiply(double scalar, Matrix matrix)
        {
            return Matrix.Multiply(matrix, scalar);
        }
        public static Matrix Multiply(Matrix left, Matrix right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckMultipliable(left, right);
            var result = new Matrix(left._rowCount, right._colCount);
            //todo
            return result;
        }
        public static Vector Multiply(Matrix matrix, Vector vector)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckMultipliable(matrix, vector);
            var result = new Vector(vector.Dimension);
            for (int i = 0; i < matrix._rowCount; i++)
            {
                var temp = 0d;
                for (int j = 0; j < matrix._colCount; j++)
                {
                    temp += matrix.Get(i, j) * vector.Get(j);
                }
                result.Set(i, temp);
            }
            return result;
        }
        public static Vector Multiply(Vector vector, Matrix matrix)
        {
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            CheckMultipliable(vector, matrix);
            var result = new Vector(vector.Dimension);
            for (int i = 0; i < matrix._colCount; i++)
            {
                var temp = 0d;
                for (int j = 0; j < matrix._rowCount; j++)
                {
                    temp += vector.Get(j) * matrix.Get(j, i);
                }
                result.Set(i, temp);
            }
            return result;
        }
        public static Matrix Divide(Matrix matrix, double scalar)
        {
            return Matrix.Multiply(matrix, 1d / scalar);
        }
        public static Matrix Divide(Matrix left, Matrix right)
        {
            //right需为可逆矩阵
            //结果为 left 乘 right逆
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            throw new System.NotImplementedException();
        }

        public void Add(double scalar)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, scalar + Get(i));
            }
        }
        public void Add(Matrix other)
        {
            if (other is null)
                ThrowHelper.ThrowArgumentNullException(nameof(other));
            CheckSameDimension(this, other);
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, Get(i) + other.Get(i));
            }
        }
        public void Subtract(double scalar)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, Get(i) - scalar);
            }
        }
        public void Subtract(Matrix other)
        {
            if (other is null)
                ThrowHelper.ThrowArgumentNullException(nameof(other));
            CheckSameDimension(this, other);
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, Get(i) - other.Get(i));
            }
        }
        public void Multiply(double scalar)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, scalar * Get(i));
            }
        }
        public void Multiply(Matrix other)
        {
            if (other is null)
                ThrowHelper.ThrowArgumentNullException(nameof(other));
            CheckSameDimension(this, other);
            //todo
        }
        public void Divide(double scalar)
        {
            Multiply(1.0 / scalar);
        }
        /// <summary>
        /// 转置乘
        /// </summary>
        /// <param name="other">另一个矩阵</param>
        /// <returns></returns>
        public Matrix TransposeMultiply(Matrix other)
        {
            if (other is null)
                ThrowHelper.ThrowArgumentNullException(nameof(other));
            CheckSameRow(this, other);

            var result = new Matrix(_colCount, other._colCount);
            //todo
            return result;
        }

        #region operator
        public static implicit operator Matrix(double[,] element)
        {
            return new Matrix(element);
        }
        public static bool operator ==(Matrix left, Matrix right)
        {
            if (left is null)
                return right is null;
            if (right is null)
                return false;
            if (left._rowCount != right._rowCount || left._colCount != right._colCount)
                return false;
            for (int i = 0; i < left._elements.Length; i++)
            {
                if (right.Get(i) != right.Get(i))
                    return false;
            }
            return true;
        }
        public static bool operator !=(Matrix left, Matrix right)
        {
            return !(left == right);
        }
        public static Matrix operator +(Matrix matrix, double scalar)
        {
            return Matrix.Add(matrix, scalar);
        }
        public static Matrix operator +(double scalar, Matrix matrix)
        {
            return Matrix.Add(scalar, matrix);
        }
        public static Matrix operator +(Matrix left, Matrix right)
        {
            return Matrix.Add(left, right);
        }
        public static Matrix operator -(Matrix matrix)
        {
            return Matrix.Negate(matrix);
        }
        public static Matrix operator -(Matrix matrix, double scalar)
        {
            return Matrix.Subtract(matrix, scalar);
        }
        public static Matrix operator -(double scalar, Matrix matrix)
        {
            return Matrix.Subtract(scalar, matrix);
        }
        public static Matrix operator -(Matrix left, Matrix right)
        {
            return Matrix.Subtract(left, right);
        }
        public static Matrix operator *(Matrix matrix, double scalar)
        {
            return Matrix.Multiply(matrix, scalar);
        }
        public static Matrix operator *(double scalar, Matrix matrix)
        {
            return Matrix.Multiply(scalar, matrix);
        }
        public static Matrix operator *(Matrix left, Matrix right)
        {
            return Matrix.Multiply(left, right);
        }
        public static Vector operator *(Matrix matrix, Vector vector)
        {
            return Matrix.Multiply(matrix, vector);
        }
        public static Vector operator *(Vector vector, Matrix matrix)
        {
            return Matrix.Multiply(vector, matrix);
        }
        public static Matrix operator /(Matrix matrix, double scalar)
        {
            return Matrix.Divide(matrix, scalar);
        }
        #endregion
    }
}
