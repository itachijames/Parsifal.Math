namespace Parsifal.Math.Algebra
{
    public partial class Vector
    {
        #region static
        public static Vector Negate(Vector vector)
        {
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            var data = new double[vector._elements.Length];
            LogicControl.LogicProvider.ScalarArray(-1, vector._elements, data);
            return data;
        }
        public static Vector Add(Vector left, Vector right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            var data = new double[left._elements.Length];
            LogicControl.LogicProvider.AddArray(left._elements, right._elements, data);
            return data;
        }
        public static Vector Subtract(Vector left, Vector right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            var data = new double[left._elements.Length];
            LogicControl.LogicProvider.SubtractArray(left._elements, right._elements, data);
            return data;
        }
        public static Vector Multiply(Vector vector, double scalar)
        {
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            var data = new double[vector._elements.Length];
            LogicControl.LogicProvider.ScalarArray(scalar, vector._elements, data);
            return data;
        }
        public static Vector Multiply(double scalar, Vector vector)
        {
            return Vector.Multiply(vector, scalar);
        }
        public static Vector Divide(Vector vector, double scalar)
        {
            return Multiply(vector, 1.0 / scalar);
        }
        public static double DotProduct(Vector left, Vector right)
        {
            if (left is null)
                ThrowHelper.ThrowArgumentNullException(nameof(left));
            if (right is null)
                ThrowHelper.ThrowArgumentNullException(nameof(right));
            CheckSameDimension(left, right);
            double result = 0d;
            LogicControl.LogicProvider.VectorDotProduct(left._elements, right._elements, result);
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
        public void Add(Vector vector)
        {
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckSameDimension(this, vector);
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, Get(i) + vector.Get(i));
            }
        }
        public void Subtract(Vector vector)
        {
            if (vector is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vector));
            CheckSameDimension(this, vector);
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, Get(i) - vector.Get(i));
            }
        }
        public void Multiply(double scalar)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                Set(i, scalar * Get(i));
            }
        }
        public void Divide(double scalar)
        {
            Multiply(1.0 / scalar);
        }
        /// <summary>
        /// 向量点积/内积
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>数量积</returns>
        public double DotProduct(Vector vector)
        {
            return Vector.DotProduct(this, vector);
        }
        #endregion

        #region operator
        public static implicit operator Vector(double[] element)
        {
            return new Vector(element);
        }
        public static bool operator ==(Vector left, Vector right)
        {
            if (left is null)
                return right is null;
            if (right is null)//此时left不为null
                return false;
            if (left._elements.Length != right._elements.Length)
                return false;
            for (int i = 0; i < left._elements.Length; i++)
            {
                if (left.Get(i) != right.Get(i))
                    return false;
            }
            return true;
        }
        public static bool operator !=(Vector left, Vector right)
        {
            return !(left == right);
        }
        public static Vector operator +(Vector left, Vector right)
        {
            return Vector.Add(left, right);
        }
        public static Vector operator -(Vector vector)
        {
            return Vector.Negate(vector);
        }
        public static Vector operator -(Vector left, Vector right)
        {
            return Vector.Subtract(left, right);
        }
        public static Vector operator *(Vector vector, double scalar)
        {
            return Vector.Multiply(vector, scalar);
        }
        public static Vector operator *(double scalar, Vector vector)
        {
            return Vector.Multiply(scalar, vector);
        }
        public static double operator *(Vector left, Vector right)
        {
            return Vector.DotProduct(left, right);
        }
        public static Vector operator /(Vector vector, double scalar)
        {
            return Vector.Divide(vector, scalar);
        }
        #endregion
    }
}
