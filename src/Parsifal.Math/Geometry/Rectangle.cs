namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// 矩形
    /// </summary>
    public readonly struct Rectangle : IPolygon
    {
        #region field
        private readonly Point2D[] _vertexes;
        #endregion

        #region constructor
        /// <summary>
        /// 构造矩形
        /// </summary>
        /// <remarks>参数点需为顺序点，且构成的邻边需垂直</remarks>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Rectangle(Point2D p1, Point2D p2, Point2D p3)
        {
            if (!(p2 - p1).IsVertical(p3 - p2))//需保证边p1p2与p2p3垂直
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(p3));
            var p4 = new Point2D(p1.X + p3.X - p2.X, p1.Y + p3.Y - p2.Y);//计算推断出p4点根据中心
            _vertexes = new Point2D[] { p1, p2, p3, p4 };
        }
        /// <summary>
        /// 构造矩形
        /// </summary>
        /// <remarks>构建的矩形临边分别平行于X、Y轴</remarks>
        /// <param name="left">左</param>
        /// <param name="right">右</param>
        /// <param name="bottom">下</param>
        /// <param name="top">上</param>
        public Rectangle(double left, double right, double bottom, double top)
        {
            if (right <= left)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(right));
            if (top <= bottom)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(top));
            var vertexes = new Point2D[]
            {
                new Point2D(bottom, left),
                new Point2D(bottom, right),
                new Point2D(top, right),
                new Point2D(top, left)
            };
            _vertexes = vertexes;
        }
        #endregion

        #region IPolygon
        public int EdgeNumber => 4;
        public Point2D[] GetVertexes()
        {
            return _vertexes;
        }
        public bool IsConvex() => true;
        public double GetArea()
        {
            //邻边的积
            return _vertexes[0].Distance(_vertexes[1]) * _vertexes[1].Distance(_vertexes[2]);
        }
        public double GetPerimeter()
        {
            //邻边和的两倍
            return (_vertexes[0].Distance(_vertexes[1]) + _vertexes[1].Distance(_vertexes[2])) * 2;
        }
        #endregion

        #region public
        /// <summary>
        /// 矩形中心
        /// </summary>
        /// <returns></returns>
        public Point2D Center()
        {
            //对角点中点
            return new Point2D((_vertexes[0].X + _vertexes[2].X) / 2, (_vertexes[0].Y + _vertexes[2].Y) / 2);
        }
        /// <summary>
        /// 是否为正方形
        /// </summary>
        /// <returns></returns>
        public bool IsSquare()
        {
            //邻边同长
            return _vertexes[0].Distance(_vertexes[1]).IsEqual(_vertexes[1].Distance(_vertexes[2]));
        }
        #endregion

        #region static
        /// <summary>
        /// 合法性检查
        /// </summary>
        /// <param name="vertexes">顶点</param>
        /// <returns>若能构成矩形则返回true;否则false</returns>
        public static bool ValidityCheck(params Point2D[] vertexes)
        {
            if (vertexes == null || vertexes.Length != 4)
                return false;
            if (UtilityHelper.HaveRepeated(vertexes))
                return false;
            //邻边需垂直
            if (!(vertexes[1] - vertexes[0]).IsVertical(vertexes[2] - vertexes[1]))
                return false;
            if (!(vertexes[2] - vertexes[1]).IsVertical(vertexes[3] - vertexes[2]))
                return false;
            //对边长相等
            if (vertexes[0].Distance(vertexes[3]).IsEqual(vertexes[1].Distance(vertexes[2])))
                return false;
            return true;
        }
        #endregion
    }
}
