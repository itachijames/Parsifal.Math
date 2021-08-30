namespace Parsifal.Math.Algebra
{
    partial class Vector<T>
    {
        #region static
        public static Vector<T> Negate(Vector<T> vector)
        {
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            T[] data = new T[vector._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayMultiply(-1, vector._elements, data);
            return data;
        }
        public static Vector<T> Add(Vector<T> left, Vector<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            T[] data = new T[left._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayAdd(left._elements, right._elements, data);
            return data;
        }
        public static Vector<T> Subtract(Vector<T> left, Vector<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            T[] data = new T[left._elements.Length];
            LogicControl.LinearAlgebraProvider.ArraySubtract(left._elements, right._elements, data);
            return data;
        }
        public static Vector<T> Multiply(Vector<T> vector, T scalar)
        {
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            T[] data = new T[vector._elements.Length];
            LogicControl.LinearAlgebraProvider.ArrayMultiply(scalar, vector._elements, data);
            return data;
        }
        public static Vector<T> Multiply(T scalar, Vector<T> vector)
        {
            return Vector<T>.Multiply(vector, scalar);
        }
        public static Vector<T> Divide(Vector<T> vector, T scalar)
        {
            if (scalar.Equals(Zero))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.ZeroParameter, nameof(scalar));
            return Vector<T>.Multiply(vector, 1.0 / scalar);
        }
        public static T DotProduct(Vector<T> left, Vector<T> right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            return LogicControl.LinearAlgebraProvider.VectorDotProduct(left._elements, right._elements);
        }
        #endregion

        #region instance
        public Vector<T> Negate()
        {
            return Vector<T>.Negate(this);
        }
        public Vector<T> Add(Vector<T> vector)
        {
            return Vector<T>.Add(this, vector);
        }
        public Vector<T> Subtract(Vector<T> vector)
        {
            return Vector<T>.Subtract(this, vector);
        }
        public Vector<T> Multiply(T scalar)
        {
            return Vector<T>.Multiply(this, scalar);
        }
        public Vector<T> Multiply(Matrix<T> matrix)
        {
            return Matrix<T>.Multiply(this, matrix);
        }
        public Vector<T> Divide(T scalar)
        {
            return Vector<T>.Divide(this, scalar);
        }
        /// <summary>
        /// 向量点积/内积
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>数量积</returns>
        public T DotProduct(Vector<T> vector)
        {
            return Vector<T>.DotProduct(this, vector);
        }
        #endregion

        #region operator
        public static implicit operator Vector<T>(T[] element)
        {//隐式转换
            return new Vector<T>(element);
        }
        public static bool operator ==(Vector<T> left, Vector<T> right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }
        public static bool operator !=(Vector<T> left, Vector<T> right)
        {
            return !(left == right);
        }
        public static Vector<T> operator +(Vector<T> left, Vector<T> right)
        {
            return Vector<T>.Add(left, right);
        }
        public static Vector<T> operator -(Vector<T> vector)
        {
            return Vector<T>.Negate(vector);
        }
        public static Vector<T> operator -(Vector<T> left, Vector<T> right)
        {
            return Vector<T>.Subtract(left, right);
        }
        public static Vector<T> operator *(Vector<T> vector, T scalar)
        {
            return Vector<T>.Multiply(vector, scalar);
        }
        public static Vector<T> operator *(T scalar, Vector<T> vector)
        {
            return Vector<T>.Multiply(scalar, vector);
        }
        public static T operator *(Vector<T> left, Vector<T> right)
        {
            return Vector<T>.DotProduct(left, right);
        }
        public static Vector<T> operator /(Vector<T> vector, T scalar)
        {
            return Vector<T>.Divide(vector, scalar);
        }
        #endregion
    }
}
