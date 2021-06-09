namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// 简单多边形
    /// </summary>
    /// <remarks>封闭、连通、边不自交</remarks>
    public readonly struct Polygon : IPolygon
    {
        #region field
        private readonly Point2D[] _vertexes;
        #endregion

        #region constructor
        /// <summary>
        /// 构造多边形
        /// </summary>
        /// <param name="vertexes">顶点</param>
        public Polygon(Point2D[] vertexes)
        {
            if (!ValidityCheck(vertexes))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(vertexes));
            _vertexes = vertexes;
        }
        #endregion

        #region IPolygon
        public int EdgeNumber => _vertexes.Length;
        public Point2D[] GetVertexes()
        {
            return _vertexes;
        }
        public bool IsConvex()
        {
            return Parsifal.Math.Algorithm.GeometryAlgorithm.IsConvexPolygon(_vertexes);
        }
        public double GetArea()
        {
            //s = |∑{X(i) * Y(i+1) - X(i+1) * Y(i)}| / 2
            //优化：各顶点减去第一个顶点后再运算，避免应多边形离原点过远且面积很小时的异常结果
            Point2D current, next;
            current = _vertexes[0];
            int count = _vertexes.Length;
            double sum = 0d;
            for (int i = 1; i <= count; i++)
            {
                next = _vertexes[i % count];
                sum += current.X * next.Y - next.X * current.Y;
                current = next;
            }
            return System.Math.Abs(sum) / 2;
        }
        public double GetPerimeter()
        {
            double sum = 0;
            Point2D pA, pB;
            pA = _vertexes[0];
            for (int i = 1; i <= _vertexes.Length; i++)
            {
                pB = _vertexes[i % _vertexes.Length];
                sum += pA.Distance(pB);
                pA = pB;
            }
            return sum;
        }
        #endregion

        #region public
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public Point2D GetVertex(int index)
        {
            if (index < 0 || index >= _vertexes.Length)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(index));
            return _vertexes[index];
        }
        /// <summary>
        /// 获取边
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public Segment GetEdge(int index)
        {
            if (index < 0 || index >= _vertexes.Length)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(index));
            return new Segment(_vertexes[index], _vertexes[(index + 1) % _vertexes.Length]);
        }
        #endregion

        #region static
        public static explicit operator Point2D[](Polygon polygon)
        {
            return polygon._vertexes;
        }
        /// <summary>
        /// 有效性检测
        /// </summary>
        /// <param name="vertexes">顶点</param>
        /// <returns>若能构成多边形则返回true;否则false</returns>
        public static bool ValidityCheck(params Point2D[] vertexes)
        {
            if (vertexes == null || vertexes.Length < 3)
                return false;
            if (UtilityHelper.HaveRepeated(vertexes))
                return false;
            //判断点集是否能构成简单多边形(相邻三点不共线，边不自交)
            for (int i = 0, j, k; i < vertexes.Length; i++)
            {
                j = (i == vertexes.Length - 1) ? 0 : i + 1;
                k = (j == vertexes.Length - 1) ? 0 : j + 1;
                if (Parsifal.Math.Algorithm.GeometryAlgorithm.IsCollinearity(vertexes[i], vertexes[j], vertexes[k]))
                    return false;
            }
            if (Parsifal.Math.Algorithm.GeometryAlgorithm.IsSelfIntersected(vertexes))
                return false;
            return true;
        }
        #endregion
    }
}
