namespace Parsifal.Math.Algebra
{
    public partial class Matrix
    {
        #region static
        public static Matrix Negate(Matrix matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            double[] data = new double[matrix._elements.Length];
            LogicControl.LogicProvider.ArrayMultiply(-1, matrix._elements, data);
            return new Matrix(matrix._rowCount, matrix._colCount, data);
        }
        public static Matrix Add(Matrix matrix, double scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            double[] data = new double[matrix._elements.Length];
            LogicControl.LogicProvider.ArrayAdd(scalar, matrix._elements, data);
            return new Matrix(matrix._rowCount, matrix._colCount, data);
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
            double[] data = new double[left._elements.Length];
            LogicControl.LogicProvider.ArrayAdd(left._elements, right._elements, data);
            return new Matrix(left._rowCount, left._colCount, data);
        }
        public static Matrix Subtract(Matrix matrix, double scalar)
        {
            return Matrix.Add(matrix, -1 * scalar);
        }
        public static Matrix Subtract(double scalar, Matrix matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            double[] temp = new double[matrix._elements.Length];
            double[] data = new double[matrix._elements.Length];
            LogicControl.LogicProvider.ArrayMultiply(-1, matrix._elements, temp);
            LogicControl.LogicProvider.ArrayAdd(scalar, temp, data);
            return new Matrix(matrix._rowCount, matrix._colCount, data);
        }
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            double[] data = new double[left._elements.Length];
            LogicControl.LogicProvider.ArraySubtract(left._elements, right._elements, data);
            return new Matrix(left._rowCount, left._colCount, data);
        }
        public static Matrix Multiply(Matrix matrix, double scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            double[] data = new double[matrix._elements.Length];
            LogicControl.LogicProvider.ArrayMultiply(scalar, matrix._elements, data);
            return new Matrix(matrix._rowCount, matrix._colCount, data);
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
            double[] data = new double[left._rowCount * right._colCount];
            LogicControl.LogicProvider.MatrixMultiply(left._rowCount, left._colCount, left._elements,
                right._rowCount, right._colCount, right._elements,
                data);
            return new Matrix(left._rowCount, right._colCount, data);
        }
        public static Vector Multiply(Matrix matrix, Vector vector)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckMultipliable(matrix, vector);
            double[] data = new double[matrix._rowCount];
            LogicControl.LogicProvider.MatrixMultiply(matrix._rowCount, matrix._colCount, matrix._elements,
                vector.Dimension, 1, vector.Storage,
                data);
            return new Vector(data);
        }
        public static Vector Multiply(Vector vector, Matrix matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckMultipliable(vector, matrix);
            double[] data = new double[matrix._colCount];

            for (int i = 0; i < matrix._colCount; i++)
            {
                double temp = 0d;
                for (int j = 0; j < matrix._rowCount; j++)
                {
                    temp += matrix.Get(j, i) * vector.Get(j);
                }
                data[i] = temp;
            }

            return new Vector(data);
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
        /// <summary>
        /// <paramref name="left"/>乘以<paramref name="right"/>的转置
        /// </summary>
        public static Matrix MultiplyTranspose(Matrix left, Matrix right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameColumn(left, right);
            var result = new Matrix(left._rowCount, right._rowCount);
            for (int i = 0; i < right._rowCount; i++)
            {
                for (int j = 0; j < left._rowCount; j++)
                {
                    double temp = 0d;
                    for (int k = 0; k < left._colCount; k++)
                    {
                        temp += left.Get(j, k) * right.Get(i, k);
                    }
                    result.Set(j, i, temp);
                }
            }
            return result;
        }
        /// <summary>
        /// <paramref name="left"/>的转置乘以<paramref name="right"/>
        /// </summary>
        public static Matrix TransposeMultiply(Matrix left, Matrix right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameRow(left, right);
            var result = new Matrix(left._colCount, right._colCount);
            for (int i = 0; i < right._colCount; i++)
            {
                for (int j = 0; j < left._colCount; j++)
                {
                    double temp = 0d;
                    for (int k = 0; k < left._rowCount; k++)
                    {
                        temp += left.Get(k, j) * right.Get(k, i);
                    }
                    result.Set(j, i, temp);
                }
            }
            return result;
        }
        #endregion

        #region instance
        public Matrix Negate()
        {
            return Matrix.Negate(this);
        }
        public Matrix Add(double scalar)
        {
            return Matrix.Add(this, scalar);
        }
        public Matrix Add(Matrix other)
        {
            return Matrix.Add(this, other);
        }
        public Matrix Subtract(double scalar)
        {
            return Matrix.Subtract(this, scalar);
        }
        public Matrix Subtract(Matrix other)
        {
            return Matrix.Subtract(this, other);
        }
        public Matrix Multiply(double scalar)
        {
            return Matrix.Multiply(this, scalar);
        }
        public Matrix Multiply(Matrix matrix)
        {
            return Matrix.Multiply(this, matrix);
        }
        public Vector Multiply(Vector vector)
        {
            return Matrix.Multiply(this, vector);
        }
        public Matrix Divide(double scalar)
        {
            return Matrix.Divide(this, scalar);
        }
        public Matrix TransposeAndMultiply(Matrix matrix)
        {
            return Matrix.TransposeMultiply(this, matrix);
        }
        public Matrix MultiplyTransposeOf(Matrix matrix)
        {
            return Matrix.MultiplyTranspose(this, matrix);
        }
        #endregion

        #region operator
        public static explicit operator Matrix(double[,] element)
        {//显式转换
            return Matrix.CreateByArray(element);
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
