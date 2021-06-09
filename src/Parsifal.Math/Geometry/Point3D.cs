using System;

namespace Parsifal.Math.Geometry
{
    using Math = System.Math;
    /// <summary>
    /// 三维点
    /// </summary>
    public struct Point3D : IEquatable<Point3D>
    {
        #region property
        /// <summary>
        /// 原点
        /// </summary>
        public static readonly Point3D Origin = new Point3D(0d, 0d, 0d);
        /// <summary>
        /// 无效点/无意义点
        /// </summary>
        public static readonly Point2D NotPoint;
        private double _x;
        private double _y;
        private double _z;
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
        /// <summary>
        /// Z坐标
        /// </summary>
        public double Z
        {
            get => _z;
            set => _z = value;
        }
        #endregion

        #region constructor
        /// <summary>
        /// 构造新三维点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        #endregion

        #region IEquatable
        public bool Equals(Point3D point) => this == point;
        #endregion

        #region BCL
        public override bool Equals(object obj) => obj is Point3D point && Equals(point);
        public override int GetHashCode() => HashCode.Combine(_x, _y, _z);
        public override string ToString() => $"({_x.ToString(UtilityHelper.DoubleFormat)}, {_y.ToString(UtilityHelper.DoubleFormat)}, {_z.ToString(UtilityHelper.DoubleFormat)})";
        public void Deconstruct(out double x, out double y, out double z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        #endregion

        #region public
        /// <summary>到指定点的距离</summary>
        /// <param name="point">目标点</param>
        /// <returns>距离</returns>
        public double Distance(Point3D point)
        {
            return Math.Sqrt(Math.Pow(_x - point._x, 2) + Math.Pow(_y - point._y, 2) + Math.Pow(_z - point._z, 2));
        }
        /// <summary>
        /// 偏移
        /// </summary>
        /// <param name="offsetX">x向偏移量</param>
        /// <param name="offsetY">y向偏移量</param>
        /// <param name="offsetZ">z向偏移量</param>
        public void Offset(double offsetX, double offsetY, double offsetZ)
        {
            _x += offsetX;
            _y += offsetY;
            _z += offsetZ;
        }
        /// <summary>
        /// 偏移
        /// </summary>
        /// <param name="vector">偏移方向</param>
        public void Offset(Vector3D vector)
        {
            Offset(vector.VX, vector.VY, vector.VZ);
        }
        #endregion

        #region operator
        /// <summary>
        /// 原点指向本点的向量
        /// </summary>
        /// <param name="point"></param>
        public static explicit operator Vector3D(Point3D point)
        {
            return new Vector3D(point._x, point._y, point._z);
        }
        public static Point3D operator +(Point3D point, Vector3D vector)
        {
            return new Point3D(point._x + vector.VX, point._y + vector.VY, point._z + vector.VZ);
        }
        public static Point3D operator -(Point3D point, Vector3D vector)
        {
            return new Point3D(point._x - vector.VX, point._y - vector.VY, point._z - vector.VZ);
        }
        public static Vector3D operator -(Point3D left, Point3D right)
        {
            return new Vector3D(left._x - right._x, left._y - right._y, left._z - right._z);
        }
        public static bool operator ==(Point3D point1, Point3D point2)
        {
            return point1._x == point2._x && point1._y == point2._y && point1._z == point2._z;
        }
        public static bool operator !=(Point3D point1, Point3D point2)
        {
            return point1._x != point2._x || point1._y != point2._y || point1._z != point2._z;
        }
        #endregion
    }
}
