using System.Collections.Concurrent;
using System.Threading.Tasks;

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
            LogicControl.LogicProvider.ScalarArray(-1, matrix._elements, data);
            return new Matrix(matrix._rowCount, matrix._colCount, data, false);
        }
        public static Matrix Add(Matrix matrix, double scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            double[] data = new double[matrix._elements.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = matrix.Get(i) + scalar;
            }
            return new Matrix(matrix._rowCount, matrix._colCount, data, false);
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
            LogicControl.LogicProvider.AddArray(left._elements, right._elements, data);
            return new Matrix(left._rowCount, left._colCount, data, false);
        }
        public static Matrix Subtract(Matrix matrix, double scalar)
        {
            return Matrix.Add(matrix, -1 * scalar);
        }
        public static Matrix Subtract(double scalar, Matrix matrix)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            double[] data = new double[matrix._elements.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = scalar - matrix.Get(i);
            }
            return new Matrix(matrix._rowCount, matrix._colCount, data, false);
        }
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            double[] data = new double[left._elements.Length];
            LogicControl.LogicProvider.SubtractArray(left._elements, right._elements, data);
            return new Matrix(left._rowCount, left._colCount, data, false);
        }
        public static Matrix Multiply(Matrix matrix, double scalar)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            double[] data = new double[matrix._elements.Length];
            LogicControl.LogicProvider.ScalarArray(scalar, matrix._elements, data);
            return new Matrix(matrix._rowCount, matrix._colCount, data, false);
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
            if (ParallelHelper.ShouldNotUseParallelism() || left.ShouldNotUseParallel() || right.ShouldNotUseParallel())
            {//直算
                for (int i = 0; i < left._rowCount; i++)
                {
                    for (int j = 0; j < right._colCount; j++)
                    {
                        double temp = 0d;
                        for (int k = 0; k < left._colCount; k++)
                        {
                            temp += left.Get(i, k) * right.Get(k, j);
                        }
                        result.Set(i, j, temp);
                    }
                }
            }
            else
            {//并行
                Parallel.ForEach(Partitioner.Create(0, left._rowCount, 1),//按左矩阵的每行来算
                    ParallelHelper.CreateParallelOptions(),
                    range =>
                    {
                        double[] row;
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            row = left.GetRowArray(i);
                            for (int j = 0; j < right._colCount; j++)
                            {
                                double[] col = right.GetColumnArray(j);
                                double temp = 0d;
                                for (int k = 0; k < left._colCount; k++)
                                {
                                    temp += row[k] * col[k];
                                }
                                result.Set(i, j, temp);
                            }
                        }
                    });
            }
            return result;
        }
        public static Vector Multiply(Matrix matrix, Vector vector)
        {
            if (matrix is null)
                ThrowHelper.ThrowArgumentNullException(nameof(matrix));
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckMultipliable(matrix, vector);
            double[] data = new double[matrix._rowCount];
            for (int i = 0; i < matrix._rowCount; i++)
            {
                double temp = 0d;
                for (int j = 0; j < matrix._colCount; j++)
                {
                    temp += matrix.Get(i, j) * vector.Get(j);
                }
                data[i] = temp;
            }
            return data;
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
            return data;
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
            if (ParallelHelper.ShouldNotUseParallelism() || left.ShouldNotUseParallel() || right.ShouldNotUseParallel())
            {
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
            }
            else
            {//并行
                throw new System.NotImplementedException();
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
            if (ParallelHelper.ShouldNotUseParallelism() || left.ShouldNotUseParallel() || right.ShouldNotUseParallel())
            {
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
            }
            else
            {//并行
                throw new System.NotImplementedException();
            }
            return result;
        }
        #endregion

        #region instance
        public void Negate()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, -1 * Get(i));
            }
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
        public Matrix Multiply(Matrix matrix)
        {
            return Matrix.Multiply(this, matrix);
        }
        public Vector Multiply(Vector vector)
        {
            return Matrix.Multiply(this, vector);
        }
        public void Divide(double scalar)
        {
            Multiply(1.0 / scalar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
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
