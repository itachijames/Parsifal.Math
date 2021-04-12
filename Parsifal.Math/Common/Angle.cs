using System;

namespace Parsifal.Math
{
    using Math = System.Math;
    /// <summary>
    /// 角度
    /// </summary>
    public readonly struct Angle : IComparable, IComparable<Angle>, IEquatable<Angle>
    {
        #region property
        /// <summary>
        /// (内部用)弧度
        /// </summary>
        private readonly double _radian;
        /// <summary>
        /// 弧度
        /// </summary>
        public double Radians { get { return _radian; } }
        /// <summary>
        /// 角度
        /// </summary>
        public double Degrees { get { return ConversionHelper.RadianToDegree(_radian); } }
        /// <summary>
        /// 分
        /// </summary>
        public double Minutes
        {
            get
            {
                if (Degrees < 0)
                {
                    var temp = Math.Ceiling(Degrees);
                    return (Degrees - temp) * 60d;
                }
                else
                {
                    var temp = Math.Floor(Degrees);
                    return (Degrees - temp) * 60d;
                }
            }
        }
        /// <summary>
        /// 秒
        /// </summary>
        public double Seconds
        {
            get
            {
                if (Degrees < 0)
                {
                    var df = Math.Ceiling(Degrees);
                    var m = (Degrees - df) * 60d;
                    var mf = Math.Ceiling(m);
                    return (m - mf) * 60d;
                }
                else
                {
                    var df = Math.Floor(Degrees);
                    var m = (Degrees - df) * 60d;
                    var mf = Math.Floor(m);
                    return (m - mf) * 60d;
                }
            }
        }
        /// <summary>
        /// 角度类型
        /// </summary>
        public AngleType Type
        {
            get
            {
                var angle = ConversionHelper.RadianWrapTo2Pi(_radian);
                if (angle.IsLess(Math.PI / 2d))
                    return AngleType.Acute;
                else if (angle.IsEqual(Math.PI / 2d))
                    return AngleType.Right;
                else if (angle.IsGreater(Math.PI / 2d))
                {
                    if (angle.IsLess(Math.PI))
                        return AngleType.Obtuse;
                    else if (angle.IsEqual(Math.PI))
                        return AngleType.Straight;
                    else if (angle.IsGreater(Math.PI))
                    {
                        if (angle.IsEqual(Math.PI * 2d))
                            return AngleType.Rotation;
                        else
                            return AngleType.Reflex;
                    }
                }
                throw new Exception("未知类型");
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 构造角度
        /// </summary>
        /// <param name="angle">弧度</param>
        public Angle(double angle)
        {
            _radian = angle;
        }
        #endregion

        #region public
        public int CompareTo(Angle other)
        {
            if (_radian > other._radian)
                return 1;
            else if (_radian < other._radian)
                return -1;
            return 0;
        }
        public int CompareTo(object obj)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(nameof(obj));
            if (!(obj is Angle))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.WrongType, nameof(obj));
            return CompareTo((Angle)obj);
        }
        public bool Equals(Angle other) => this == other;
        public override bool Equals(object obj) => obj is Angle angle && Equals(angle);
        public override int GetHashCode() => _radian.GetHashCode();
        public override string ToString() => Degrees.ToString("0.##°");
        #endregion

        #region operator 
        public static bool operator ==(Angle left, Angle right)
        {
            return left._radian.IsEqual(right._radian);
        }
        public static bool operator !=(Angle left, Angle right)
        {
            return !left._radian.IsEqual(right._radian);
        }
        public static bool operator <(Angle left, Angle right)
        {
            return left._radian.IsLess(right._radian);
        }
        public static bool operator >(Angle left, Angle right)
        {
            return left._radian.IsGreater(right._radian);
        }
        public static bool operator <=(Angle left, Angle right)
        {
            return left._radian.IsLessOrEqual(right._radian);
        }
        public static bool operator >=(Angle left, Angle right)
        {
            return left._radian.IsGreaterOrEqual(right._radian);
        }
        public static Angle operator -(Angle value)
        {
            return new Angle(-1 * value._radian);
        }
        public static Angle operator +(Angle left, Angle right)
        {
            return new Angle(left._radian + right._radian);
        }
        public static Angle operator -(Angle left, Angle right)
        {
            return new Angle(left._radian - right._radian);
        }
        public static Angle operator *(Angle angle, double scalar)
        {
            return new Angle(scalar * angle._radian);
        }
        public static Angle operator *(double scalar, Angle angle)
        {
            return new Angle(scalar * angle._radian);
        }
        public static Angle operator /(Angle angle, double scalar)
        {
            return new Angle(angle._radian / scalar);
        }
        #endregion
    }
}
