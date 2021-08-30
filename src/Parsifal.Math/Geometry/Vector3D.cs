using System;

namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// 三维向量
    /// </summary>
    public struct Vector3D : IEquatable<Vector3D>
    {
        #region property
        /// <summary>
        /// 零向量
        /// </summary>
        public static readonly Vector3D Zero = new Vector3D(0d, 0d, 0d);
        /// <summary>
        /// X单位向量
        /// </summary>
        public static readonly Vector3D UnitX = new Vector3D(1d, 0d, 0d);
        /// <summary>
        /// Y单位向量
        /// </summary>
        public static readonly Vector3D UnitY = new Vector3D(0d, 1d, 0d);
        /// <summary>
        /// Z单位向量
        /// </summary>
        public static readonly Vector3D UnitZ = new Vector3D(0d, 0d, 1d);
        private double _x;
        private double _y;
        private double _z;
        /// <summary>
        /// X向
        /// </summary>
        public double VX
        {
            readonly get => _x;
            set => _x = value;
        }
        /// <summary>
        /// Y向
        /// </summary>
        public double VY
        {
            readonly get => _y;
            set => _y = value;
        }
        /// <summary>
        /// Z向
        /// </summary>
        public double VZ
        {
            readonly get => _z;
            set => _z = value;
        }
        /// <summary>
        /// 模
        /// </summary>
        public double Norm
        {
            get { return System.Math.Sqrt(System.Math.Pow(_x, 2) + System.Math.Pow(_y, 2) + System.Math.Pow(_z, 2)); }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 构造三维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        #endregion

        #region IEquatable
        public bool Equals(Vector3D vector) => this == vector;
        #endregion

        #region BCL
        public override bool Equals(object obj) => obj is Vector3D vector && Equals(vector);
        public override int GetHashCode() => System.HashCode.Combine(_x, _y, _z);
        public override string ToString() => $"[{_x.ToString(UtilityHelper.DigitalFormat)}, {_y.ToString(UtilityHelper.DigitalFormat)}, {_z.ToString(UtilityHelper.DigitalFormat)}]";
        public void Deconstruct(out double x, out double y, out double z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        #endregion

        #region static
        /// <summary>
        /// 向量点乘
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>内积/数量积/点积</returns>
        public static double DotProduct(Vector3D left, Vector3D right)
        {
            return left._x * right._x + left._y * right._y + left._z * right._z;
        }
        /// <summary>
        /// 向量叉乘(外积/向量积)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>外积/向量积/叉积</returns>
        public static Vector3D CrossProduct(Vector3D left, Vector3D right)
        {
            return new Vector3D
            {
                VX = left._y * right._z - left._z * right._y,
                VY = left._z * right._x - left._x * right._z,
                VZ = left._x * right._y - left._y * right._x
            };
        }
        /// <summary>
        /// 标准化向量
        /// </summary>
        /// <remarks>同向、长为1</remarks>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3D Normalize(Vector3D vector)
        {
            return vector / vector.Norm;
        }
        /// <summary>
        /// 向量关于法向量的反射向量
        /// </summary>
        /// <param name="vector">原向量</param>
        /// <param name="normal">法向量</param>
        /// <returns>反射向量</returns>
        public static Vector3D Reflect(Vector3D vector, Vector3D normal)
        {
            var dot = DotProduct(vector, normal);
            return vector - (2 * dot * normal);
        }
        #endregion

        #region public
        /// <summary>
        /// 标准化
        /// </summary>
        public Vector3D Normalize()
        {
            return this /= Norm;
        }
        /// <summary>
        /// 反向
        /// </summary>
        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
        }
        #endregion

        #region operator
        public static explicit operator Point3D(Vector3D vector)
        {
            return new Point3D(vector._x, vector._y, vector._z);
        }
        public static Vector3D operator +(Vector3D left, Vector3D right)
        {
            return new Vector3D(left._x + right._x, left._y + right._y, left._z + right._z);
        }
        public static Point3D operator +(Vector3D vector, Point3D point)
        {
            return new Point3D(vector._x + point.X, vector._y + point.Y, vector._z + point.Z);
        }
        public static Vector3D operator -(Vector3D left, Vector3D right)
        {
            return new Vector3D(left._x - right._x, left._y - right._y, left._z - right._z);
        }
        public static Point3D operator -(Vector3D vector, Point3D point)
        {
            return new Point3D(vector._x - point.X, vector._y - point.Y, vector._z - point.Z);
        }
        public static Vector3D operator -(Vector3D vector)
        {
            return new Vector3D(-1 * vector._x, -1 * vector._y, -1 * vector._z);
        }
        public static Vector3D operator *(Vector3D vector, double scalar)
        {
            return new Vector3D(vector._x * scalar, vector._y * scalar, vector._z * scalar);
        }
        public static Vector3D operator *(double scalar, Vector3D vector)
        {
            return vector * scalar;
        }
        public static Vector3D operator /(Vector3D vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }
        public static bool operator ==(Vector3D left, Vector3D right)
        {
            return left._x == right._x && left._y == right._y && left._z == right._z;
        }
        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return left._x != right._x || left._y != right._y || left._z != right._z;
        }
        #endregion
    }
}
