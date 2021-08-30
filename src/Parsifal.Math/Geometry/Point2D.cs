using System;

namespace Parsifal.Math.Geometry
{
    using Math = System.Math;
    /// <summary>
    /// 二维点
    /// </summary>
    /// <remarks>
    /// 默认为笛卡尔坐标系(Cartesian Coordinate),直角坐标系,以第一象限为正
    /// </remarks>
    public struct Point2D : IEquatable<Point2D>
    {
        #region property
        /// <summary>
        /// 原点
        /// </summary>
        public static readonly Point2D Origin = new Point2D(0d, 0d);
        /// <summary>
        /// 无效点/无意义点
        /// </summary>
        public static readonly Point2D NotPoint;
        private double _x;
        private double _y;
        /// <summary>
        /// X坐标
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }
        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }
        #endregion

        #region constructor
        /// <summary>
        /// 构造二维点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2D(double x, double y)
        {
            this._x = x;
            this._y = y;
        }
        #endregion

        #region IEquatable
        public bool Equals(Point2D point) => this == point;
        #endregion

        #region BCL
        public override bool Equals(object obj) => obj is Point2D point && Equals(point);
        public override int GetHashCode() => HashCode.Combine(_x, _y);
        public override string ToString() => $"({_x.ToString(UtilityHelper.DigitalFormat)}, {_y.ToString(UtilityHelper.DigitalFormat)})";
        public void Deconstruct(out double x, out double y)
        {
            x = _x;
            y = _y;
        }
        #endregion

        #region public
        /// <summary>
        /// 到指定点的距离
        /// </summary>
        /// <param name="point">目标点</param>
        /// <returns></returns>
        public double Distance(Point2D point)
        {
            return Math.Sqrt(Math.Pow(_x - point._x, 2) + Math.Pow(_y - point._y, 2));
        }
        /// <summary>
        /// 偏移
        /// </summary>
        /// <param name="offectX">x向偏移量</param>
        /// <param name="offectY">y向偏移量</param>
        public void Offset(double offectX, double offectY)
        {
            this._x += offectX;
            this._y += offectY;
        }
        /// <summary>
        /// 偏移
        /// </summary>
        /// <param name="vector">偏移方向</param>
        public void Offset(Vector2D vector)
        {
            Offset(vector.VX, vector.VY);
        }
        #endregion

        #region operator
        /// <summary>
        /// 原点指向本点的向量
        /// </summary>
        /// <param name="point"></param>
        public static explicit operator Vector2D(Point2D point)
        {
            return new Vector2D(point._x, point._y);
        }
        public static Point2D operator +(Point2D point, Vector2D vector)
        {
            return new Point2D(point._x + vector.VX, point._y + vector.VY);
        }
        public static Point2D operator -(Point2D point, Vector2D vector)
        {
            return new Point2D(point._x - vector.VX, point._y - vector.VY);
        }
        public static Vector2D operator -(Point2D left, Point2D right)
        {
            return new Vector2D(left._x - right._x, left._y - right._y);
        }
        public static bool operator ==(Point2D left, Point2D right)
        {
            return left._x == right._x && left._y == right._y;
        }
        public static bool operator !=(Point2D left, Point2D right)
        {
            return left._x != right._x || left._y != right._y;
        }
        #endregion
    }
}
