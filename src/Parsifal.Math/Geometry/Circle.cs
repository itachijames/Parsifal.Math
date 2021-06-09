using System;

namespace Parsifal.Math.Geometry
{
    using Math = System.Math;
    /// <summary>
    /// 圆
    /// </summary>
    public readonly struct Circle : IEquatable<Circle>, ICloseShape
    {
        #region property
        /// <summary>
        /// 圆心
        /// </summary>
        public Point2D Center { get; }
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; }
        #endregion

        #region constructor
        /// <summary>
        /// 构造圆
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public Circle(Point2D center, double radius)
        {
            if (radius <= 0)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.NotPositiveParameter, nameof(radius));
            this.Center = center;
            this.Radius = radius;
        }
        #endregion

        #region IEquatable
        public bool Equals(Circle other) => this == other;
        #endregion

        #region ICloseShape
        public double GetArea()
        {
            return Math.PI * Math.Pow(Radius, 2);
        }
        public double GetPerimeter()
        {
            return 2 * Math.PI * Radius;
        }
        #endregion

        #region BCL
        public override bool Equals(object obj) => obj is Circle circle && Equals(circle);
        public override int GetHashCode() => HashCode.Combine(Center, Radius);
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"(X {(Center.X >= 0 ? '-' : '+')} {Math.Abs(Center.X).ToString(UtilityHelper.DoubleFormat)})² + ");
            sb.Append($"(Y {(Center.Y >= 0 ? '-' : '+')} {Math.Abs(Center.Y).ToString(UtilityHelper.DoubleFormat)})² = ");
            sb.Append($"{Radius}²");
            return sb.ToString();
        }
        #endregion

        #region operator
        public static bool operator ==(Circle left, Circle right)
        {
            return left.Center == right.Center && left.Radius == right.Radius;
        }
        public static bool operator !=(Circle left, Circle right)
        {
            return left.Center != right.Center || left.Radius != right.Radius;
        }
        #endregion
    }
}
