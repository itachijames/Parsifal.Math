using System;

namespace Parsifal.Math.Geometry
{
    using Math = System.Math;
    /// <summary>
    /// 直线
    /// </summary>
    public readonly struct Line : IEquatable<Line>
    {
        #region property
        /// <summary>
        /// X轴
        /// </summary>
        public static readonly Line XAxis = new Line(0, 1, 0);
        /// <summary>
        /// Y轴
        /// </summary>
        public static readonly Line YAxis = new Line(1, 0, 0);

        //内部采用直线一般式: Ax + By + C = 0
        //斜率k = -A / B         Y轴交点b = -C / B
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        /// <summary>
        /// 斜率
        /// </summary>
        public double Slope
        {
            get
            {
                if (_a == 0)
                    return 0d;
                else if (_b == 0)
                    return double.PositiveInfinity;
                else
                    return -1 * (_a / _b);
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 构造直线(两点式)
        /// </summary>
        /// <param name="point1">点1</param>
        /// <param name="point2">点2</param>
        public Line(Point2D point1, Point2D point2)
        {
            if (point1 == point2)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.SameParameter, nameof(point2));
            // (y-y1)/(y2-y1) = (x-x1)/(x2-x1)                
            _a = point2.Y - point1.Y;
            _b = point1.X - point2.X;
            _c = point2.X * point1.Y - point1.X * point2.Y;
        }
        /// <summary>
        /// 构造直线(斜截式)
        /// </summary>
        /// <remarks>直线表达式: y = kx + b</remarks>
        /// <param name="k">斜率</param>
        /// <param name="b">Y轴交点</param>
        public Line(double k, double b)
        {
            if (double.IsNaN(k) || double.IsInfinity(k))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(k));
            _a = k;
            _b = -1;
            _c = b;
        }
        /// <summary>
        /// 构造直线(一般式)
        /// </summary>
        /// <remarks>直线表达式: ax + by + c = 0</remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public Line(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }
        #endregion

        #region public
        /// <summary>
        /// 获取Y坐标
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <returns></returns>
        public double GetY(double x)
        {
            if (_b == 0)//平行于Y轴
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(x));
            return -1 * ((_a * x + _c) / _b);
        }
        /// <summary>
        /// 获取X坐标
        /// </summary>
        /// <param name="y">y坐标</param>
        /// <returns></returns>
        public double GetX(double y)
        {
            if (_a == 0)//平行于X轴
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(y));
            return -1 * ((_b * y + _c) / _a);
        }
        /// <summary>
        /// 是否平行
        /// </summary>
        /// <param name="line">直线</param>
        /// <returns>平行返回true;否则false</returns>
        public bool IsParallel(Line line)
        {
            if (_a == 0)
                return line._a == 0;
            else if (_b == 0)
                return line._b == 0;
            else
                return (_a * line._b).IsEqual(_b * line._a);//比较斜率是否相同，除法转乘法
        }
        /// <summary>
        /// 是否垂直
        /// </summary>
        /// <param name="line">直线</param>
        /// <returns>垂直返回true;否则false</returns>
        public bool IsVertical(Line line)
        {
            if (_a == 0)
                return line._b == 0;
            else if (_b == 0)
                return line._a == 0;
            else
                return (_a * line._a).IsEqual(-1 * (_b * line._b));//斜率积为-1
        }
        /// <summary>
        /// 点到线距离
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns></returns>
        public double Distance(Point2D point)
        {
            // d = |A*x+B*y+C|/√(A^2+B^2)
            return Math.Abs(_a * point.X + _b * point.Y + _c) / Math.Sqrt(Math.Pow(_a, 2) + Math.Pow(_b, 2));
        }
        /// <summary>
        /// 获取与直线的交点
        /// </summary>
        /// <param name="line">线</param>
        /// <returns>交点</returns>
        public Point2D GetIntersection(Line line)
        {
            if (IsParallel(line))
                return Point2D.NotPoint;
            double m = _a * line._b - _b * line._a;
            return new Point2D((_b * line._c - _c * line._b) / m, (_c * line._a - _a * line._c) / m);
        }
        /// <summary>
        /// 点到线的垂足
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns></returns>
        public Point2D GetFootpoint(Point2D point)
        {
            //X = (B^2*x-A*B*y-A*C)/(A^2+B&2)
            //Y = (A^2*y-A*B*x-B*C)/(A^2+B&2)
            double temp = Math.Pow(_a, 2) + Math.Pow(_b, 2);
            return new Point2D((Math.Pow(_b, 2) * point.X - _a * _b * point.Y - _a * _c) / temp,
                (Math.Pow(_a, 2) * point.Y - _a * _b * point.X - _b * _c) / temp);
        }
        /// <summary>
        /// 获取点关于线的对称点
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns></returns>
        public Point2D GetSymmetryPoint(Point2D point)
        {
            //X = ((B^2-A^2)*x-2*A*B*y-2*A*C)/(A^2+B^2)
            //Y = ((A^2-B^2)*y-2*A*B*x-2*B*C)/(A^2+B^2)
            double temp = Math.Pow(_a, 2) + Math.Pow(_b, 2);
            return new Point2D(((Math.Pow(_b, 2) - Math.Pow(_a, 2)) * point.X - 2 * _a * _b * point.Y - 2 * _a * _c) / temp,
                ((Math.Pow(_a, 2) - Math.Pow(_b, 2)) * point.Y - 2 * _a * _b * point.X - 2 * _b * _c) / temp);
        }
        public bool Equals(Line other) => this == other;
        public override bool Equals(object obj) => obj is Line line && Equals(line);
        public override int GetHashCode() => HashCode.Combine(_a, _b, _c);
        public override string ToString() => $"{_a:F3}X + {_b:F3}Y + {_c:F3} = 0";
        #endregion

        #region static
        public static bool operator ==(Line left, Line right)
        {
            if (left._a.IsZero())
            {//平行于X轴
                if (right._a.IsZero())
                    return (left._c * right._b).IsEqual(left._b * right._c);
                else
                    return false;
            }
            else if (left._b.IsZero())
            {//平行于Y轴
                if (right._b.IsZero())
                    return (left._c * right._a).IsEqual(left._a * right._c);
                else
                    return false;
            }
            else
            {//既不平行于X轴,也不平行于Y轴
                //先将X的系数转化为一致,再比较Y的系数和常数值
                var multiple = right._a / left._a;
                return right._b.IsEqual(left._b * multiple) && right._c.IsEqual(left._c * multiple);
            }
        }
        public static bool operator !=(Line left, Line right)
        {
            return !(left == right);
        }
        #endregion
    }
}
