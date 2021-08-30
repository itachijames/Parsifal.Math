using System;

namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// 二维向量
    /// </summary>
    public struct Vector2D : IEquatable<Vector2D>
    {
        #region property
        /// <summary>
        /// 零向量
        /// </summary>
        public static readonly Vector2D Zero = new Vector2D(0d, 0d);
        /// <summary>
        /// X单位向量
        /// </summary>
        public static readonly Vector2D UnitX = new Vector2D(1d, 0d);
        /// <summary>
        /// Y单位向量
        /// </summary>
        public static readonly Vector2D UnitY = new Vector2D(0d, 1d);
        private double _x;
        private double _y;
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
        /// 模
        /// </summary>
        public double Norm
        {
            get { return System.Math.Sqrt(System.Math.Pow(_x, 2) + System.Math.Pow(_y, 2)); }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 构造二维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2D(double x, double y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region IEquatable
        public bool Equals(Vector2D vector) => this == vector;
        #endregion

        #region BCL
        public override bool Equals(object obj) => obj is Vector2D vector && Equals(vector);
        public override int GetHashCode() => HashCode.Combine(_x, _y);
        public override string ToString() => $"[{_x.ToString(UtilityHelper.DigitalFormat)}, {_y.ToString(UtilityHelper.DigitalFormat)}]";
        public void Deconstruct(out double x, out double y)
        {
            x = _x;
            y = _y;
        }
        #endregion

        #region static
        /// <summary>
        /// 向量点乘
        /// </summary>
        /// <remarks>
        /// a • b = |a|×|b|×cos∠(a,b)
        /// </remarks>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>内积/数量积/点积</returns>
        public static double DotProduct(Vector2D left, Vector2D right)
        {
            return left._x * right._x + left._y * right._y;
        }
        /// <summary>
        /// 向量叉乘
        /// </summary>
        /// <remarks>
        /// a × b = |a|×|b|×sin∠(a,b)
        /// 注意: 二维向量叉乘结果仍为"向量"，但方向垂直于其组成的平面(也即"法向量")，故此方法结果为一"标量"，数值为它的模
        /// </remarks>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>外积/向量积/叉积</returns>
        public static double CrossProduct(Vector2D left, Vector2D right)
        {
            return left._x * right._y - left._y * right._x;
        }
        /// <summary>
        /// 标准化向量
        /// </summary>
        /// <remarks>同向、长为1</remarks>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2D Normalize(Vector2D vector)
        {
            return vector / vector.Norm;
        }
        /// <summary>
        /// 向量关于法向量的反射向量
        /// </summary>
        /// <param name="vector">原向量</param>
        /// <param name="normal">法向量</param>
        /// <returns>反射向量</returns>
        public static Vector2D Reflect(Vector2D vector, Vector2D normal)
        {
            var dot = DotProduct(vector, normal);
            return vector - (2 * dot * normal);
        }
        #endregion

        #region public
        /// <summary>
        /// 标准化
        /// </summary>
        public Vector2D Normalize()
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
        }
        /// <summary>
        /// 是否平行
        /// </summary>
        /// <param name="vector">待比较向量</param>
        /// <returns>平行时返回true;否则false</returns>
        public bool IsParallel(Vector2D vector)
        {
            if (this == Zero || vector == Zero)
                return true;
            return CrossProduct(this, vector).IsZero();
        }
        /// <summary>
        /// 是否垂直
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>垂直返回true;否则false</returns>
        public bool IsVertical(Vector2D vector)
        {
            if (this == Zero || vector == Zero)
                return true;
            return DotProduct(this, vector).IsZero();
        }
        #endregion

        #region operator
        /// <summary>
        /// 从原点起指向的点
        /// </summary>
        /// <param name="vector"></param>
        public static explicit operator Point2D(Vector2D vector)
        {
            return new Point2D(vector._x, vector._y);
        }
        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return new Vector2D(left._x + right._x, left._y + right._y);
        }
        public static Point2D operator +(Vector2D vector, Point2D point)
        {
            return new Point2D(point.X + vector._x, point.Y + vector._y);
        }
        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return new Vector2D(left._x - right._x, left._y - right._y);
        }
        public static Vector2D operator -(Vector2D vector)
        {
            return new Vector2D(-1 * vector._x, -1 * vector._y);
        }
        public static Vector2D operator *(Vector2D vector, double scalar)
        {
            return new Vector2D(vector._x * scalar, vector._y * scalar);
        }
        public static Vector2D operator *(double scalar, Vector2D vector)
        {
            return vector * scalar;
        }
        public static Vector2D operator /(Vector2D vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }
        public static bool operator ==(Vector2D left, Vector2D right)
        {
            return left._x == right._x && left._y == right._y;
        }
        public static bool operator !=(Vector2D left, Vector2D right)
        {
            return left._x != right._x || left._y != right._y;
        }
        #endregion
    }
}
