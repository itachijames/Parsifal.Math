using System;

namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// 射线
    /// </summary>
    public readonly struct Ray : IEquatable<Ray>
    {
        #region property
        //射线一般表示 p = p0 + t*u ,t>=0
        private readonly Point2D _pos;
        private readonly Vector2D _dir;
        /// <summary>
        /// 位置/起点
        /// </summary>
        public Point2D Position { get => _pos; }
        /// <summary>
        /// 方向
        /// </summary>
        public Vector2D Direction { get => _dir; }
        #endregion

        #region constructor
        /// <summary>
        /// 构造射线
        /// </summary>
        /// <param name="position">起点</param>
        /// <param name="direction">方向</param>
        public Ray(Point2D position, Vector2D direction)
        {
            _pos = position;
            _dir = direction.Normalize();
        }
        /// <summary>
        /// 构造射线
        /// </summary>
        /// <param name="position">起点</param>
        /// <param name="point">射线上任一点</param>
        public Ray(Point2D position, Point2D point)
        {
            _pos = position;
            _dir = (point - position).Normalize();
        }
        #endregion

        #region public
        public bool Equals(Ray other) => this == other;
        public override bool Equals(object obj) => obj is Ray ray && Equals(ray);
        public override int GetHashCode() => HashCode.Combine(_pos, _dir);
        public override string ToString() => $"Position:{_pos}, Direction:{_dir}";
        #endregion

        #region operator
        public static bool operator ==(Ray left, Ray right)
        {
            return left._pos == right._pos && left._dir == right._dir;
        }
        public static bool operator !=(Ray left, Ray right)
        {
            return left._pos != right._pos || left._dir != right._dir;
        }
        #endregion
    }
}
