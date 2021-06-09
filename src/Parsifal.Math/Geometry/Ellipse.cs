namespace Parsifal.Math.Geometry
{
    using Math = System.Math;
    /// <summary>
    /// 椭圆
    /// </summary>
    /// <remarks>长短轴分别平行于XY轴</remarks>
    public readonly struct Ellipse : ICloseShape
    {
        #region field
        //椭圆方程式：(x-x0)^2/a^2 + (y-y0)^2/b^2 = 1
        /// <summary>
        /// 椭圆中心
        /// </summary>
        private readonly Point2D _center;
        /// <summary>
        /// x轴半轴距
        /// </summary>
        private readonly double _a;
        /// <summary>
        /// y轴半轴距
        /// </summary>
        private readonly double _b;
        /// <summary>
        /// a是否大于b
        /// </summary>
        /// <remarks>
        /// a大于b表示长轴在X轴，a小于b表示长轴在Y轴
        /// </remarks>
        private readonly bool _isAGreat => _a > _b;
        /// <summary>
        /// 半焦距
        /// </summary>
        private readonly double _c => Math.Sqrt(Math.Abs(Math.Pow(_a, 2) - Math.Pow(_b, 2)));
        #endregion

        #region property
        /// <summary>
        /// 中心
        /// </summary>
        public Point2D Center => _center;
        /// <summary>
        /// 焦点A(左/下)
        /// </summary>
        public Point2D FocusA
        {
            get
            {
                if (_isAGreat)
                    return new Point2D(_center.X - _c, _center.Y);
                else
                    return new Point2D(_center.X, _center.Y - _c);
            }
        }
        /// <summary>
        /// 焦点B(右/上)
        /// </summary>
        public Point2D FocusB
        {
            get
            {
                if (_isAGreat)
                    return new Point2D(_center.X + _c, _center.Y);
                else
                    return new Point2D(_center.X, _center.Y + _c);
            }
        }
        /// <summary>
        /// 准线A(左/下)
        /// </summary>
        public Line DirectrixA
        {
            get
            {
                if (_isAGreat)
                    return new Line(1, 0, Math.Pow(_a, 2) / _c);//x=-a^2/c
                else
                    return new Line(0, 1, Math.Pow(_b, 2) / _c);//y=-b^2/c
            }
        }
        /// <summary>
        /// 准线B(右/上)
        /// </summary>
        public Line DirectrixB
        {
            get
            {
                if (_isAGreat)
                    return new Line(1, 0, -1 * Math.Pow(_a, 2) / _c);//x=a^2/c
                else
                    return new Line(0, 1, -1 * Math.Pow(_b, 2) / _c);//y=b^2/c
            }
        }
        /// <summary>
        /// 长半轴
        /// </summary>
        public double Semimajor { get => _isAGreat ? _a : _b; }
        /// <summary>
        /// 短半轴
        /// </summary>
        public double Semiminor { get => _isAGreat ? _b : _a; }
        /// <summary>
        /// 焦距
        /// </summary>
        public double FocalLength { get => 2 * _c; }
        /// <summary>
        /// 离心率
        /// </summary>
        public double Eccentricity { get => _c / (_isAGreat ? _a : _b); }//焦距与长轴之比
        #endregion

        #region constructor
        /// <summary>
        /// 构造椭圆
        /// </summary>
        /// <param name="a">X轴半轴距</param>
        /// <param name="b">Y轴半轴距</param>
        /// <param name="center">中心点</param>
        public Ellipse(double a, double b, Point2D center)
        {
            if (a <= 0)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.NotPositiveParameter, nameof(a));
            if (b <= 0)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.NotPositiveParameter, nameof(b));
            if (a.IsEqual(b))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.SameParameter, nameof(b));
            _a = a;
            _b = b;
            _center = center;
        }
        /// <summary>
        /// 构造椭圆
        /// </summary>
        /// <param name="left">左边界</param>
        /// <param name="right">右边界</param>
        /// <param name="bottom">下边界</param>
        /// <param name="top">上边界</param>
        public Ellipse(double left, double right, double bottom, double top)
        {
            if (right <= left)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(right));
            if (top <= bottom)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(top));
            if ((right - left) == (top - bottom))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter);
            _a = (right - left) / 2;
            _b = (top - bottom) / 2;
            _center = new Point2D((left + right) / 2, (bottom + top) / 2);
        }
        #endregion

        #region ICloseShape
        public double GetArea()
        {
            return Math.PI * _a * _b;
        }
        public double GetPerimeter()
        {
            //椭圆周长原始公式较为复杂，且不是初等公式，这里采用Ramanujan近似公式
            //c ≈ π * (a + b) * ( 1 + (3 * λ^2) / (10 + √(4 - 3 * λ^2)))
            //λ = (a - b) / ( a + b)
            double lambda = Math.Abs(_a - _b) / (_a + _b);
            return Math.PI * (_a + _b) * (1 + 3 * Math.Pow(lambda, 2) / (10 + Math.Sqrt(4 - 3 * Math.Pow(lambda, 2))));
        }
        #endregion
    }
}
