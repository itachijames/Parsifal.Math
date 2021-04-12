using System;

namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// (二维)线段
    /// </summary>
    public readonly struct Segment : IEquatable<Segment>
    {
        #region property
        /// <summary>
        /// 端点A
        /// </summary>
        public Point2D EpA { get; }
        /// <summary>
        /// 端点B
        /// </summary>
        public Point2D EpB { get; }
        /// <summary>
        /// 长度
        /// </summary>
        public double Length
        {
            get { return EpA.Distance(EpB); }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 构造线段
        /// </summary>
        /// <param name="epA">端点A</param>
        /// <param name="epB">端点B</param>
        public Segment(Point2D epA, Point2D epB)
        {
            if (epA == epB)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.SameParameter, nameof(epB));
            this.EpA = epA;
            this.EpB = epB;
        }
        #endregion

        #region static
        public static implicit operator (Point2D, Point2D)(Segment segment)
        {
            return (segment.EpA, segment.EpB);
        }
        #endregion

        #region public
        /// <summary>
        /// 线段中点
        /// </summary>
        /// <returns>线段中点</returns>
        public Point2D GetMidpoint()
        {
            return new Point2D((EpA.X + EpB.X) / 2, (EpA.Y + EpB.Y) / 2);
        }
        /// <summary>
        /// 所属直线
        /// </summary>
        /// <returns>直线</returns>
        public Line GetLine()
        {
            return new Line(EpA, EpB);
        }
        public bool Equals(Segment other) => this == other;
        public override bool Equals(object obj) => obj is Segment seg && Equals(seg);
        public override int GetHashCode() => HashCode.Combine(EpA, EpB);
        public override string ToString() => $"[{EpA} <-> {EpB}]";
        public void Deconstruct(out Point2D epA, out Point2D epB)
        {
            epA = this.EpA;
            epB = this.EpB;
        }
        #endregion

        #region operator
        public static bool operator ==(Segment first, Segment second)
        {//线段不区分明确端点
            if ((first.EpA == second.EpA && first.EpB == second.EpB)
                || (first.EpA == second.EpB && first.EpB == second.EpA))
                return true;
            return false;
        }
        public static bool operator !=(Segment first, Segment second)
        {
            return !(first == second);
        }
        #endregion
    }
}
