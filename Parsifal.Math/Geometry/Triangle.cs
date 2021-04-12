using System;

namespace Parsifal.Math.Geometry
{
    using Math = System.Math;
    /// <summary>
    /// 三角形
    /// </summary>
    public readonly struct Triangle : IPolygon
    {
        #region field
        private readonly Point2D _epA;
        private readonly Point2D _epB;
        private readonly Point2D _epC;
        #endregion

        #region constructor
        /// <summary>
        /// 构造三角形
        /// </summary>
        /// <param name="epA"></param>
        /// <param name="epB"></param>
        /// <param name="epC"></param>
        public Triangle(Point2D epA, Point2D epB, Point2D epC)
        {
            if (!ValidityCheck(epA, epB, epC))
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(epC));
            _epA = epA;
            _epB = epB;
            _epC = epC;
        }
        #endregion

        #region IPolygon
        public int EdgeNumber => 3;
        public Point2D[] GetVertexes()
        {
            return new Point2D[] { _epA, _epB, _epC };
        }
        public bool IsConvex() => true;
        public double GetArea()
        {
            //行列式法
            //    | x1 y1 1 |
            //s = | x2 y2 1 | / 2 = |(x1y2+x2y3+x3y1-x1y3-x2y1-x3y2)| / 2
            //    | x4 y3 1 |
            //
            return Math.Abs(_epA.X * _epB.Y + _epB.X * _epC.Y + _epC.X * _epA.Y - _epA.X * _epC.Y - _epB.X * _epA.Y - _epC.X * _epB.Y) / 2;
        }
        public double GetPerimeter()
        {
            double a = _epB.Distance(_epC);
            double b = _epC.Distance(_epA);
            double c = _epA.Distance(_epB);
            return a + b + c;
        }
        #endregion

        #region public
        /// <summary>
        /// 获取边长
        /// </summary>
        /// <returns>边长（小到大）</returns>
        public double[] GetEdgeLength()
        {
            double a = _epB.Distance(_epC);
            double b = _epC.Distance(_epA);
            double c = _epA.Distance(_epB);
            double[] result = new double[] { a, b, c };
            Array.Sort(result);
            return result;
        }
        /// <summary>
        /// 三角形类型
        /// </summary>
        /// <returns></returns>
        public AngleType GetTriangleType()
        {
            var edges = GetEdgeLength();
            double t1 = Math.Pow(edges[0], 2);
            double t2 = Math.Pow(edges[1], 2);
            double t3 = Math.Pow(edges[2], 2);
            double delta = t3 - t1 - t2;
            if (delta.IsZero())
                return AngleType.Right;
            if (delta.IsGreater(0))
                return AngleType.Obtuse;
            else
                return AngleType.Acute;
        }
        /// <summary>
        /// 重心
        /// </summary>
        /// <returns>重心/质心</returns>
        public Point2D Centroid()
        {
            return new Point2D((_epA.X + _epB.X + _epC.X) / 3, (_epA.Y + _epB.Y + _epC.Y) / 3);
        }
        /// <summary>
        /// 内心
        /// </summary>
        /// <returns>内心</returns>
        public Point2D Incentre()
        {
            double a = _epB.Distance(_epC);//边长
            double b = _epC.Distance(_epA);
            double c = _epA.Distance(_epB);
            return new Point2D((a * _epA.X + b * _epB.X + c + _epC.X) / (a + b + c),
                (a + _epA.Y + b * _epB.Y + c + _epC.Y) / (a + b + c));
        }
        /// <summary>
        /// 外心
        /// </summary>
        /// <returns>外心</returns>
        public Point2D Circum()
        {
            double temp1 = Math.Pow(_epA.X, 2) + Math.Pow(_epA.Y, 2);
            double temp2 = Math.Pow(_epB.X, 2) + Math.Pow(_epB.Y, 2);
            double temp3 = Math.Pow(_epC.X, 2) + Math.Pow(_epC.Y, 2);
            double temp4 = 2 * (_epA.X - _epB.X) * (_epC.Y - _epB.Y) - 2 * (_epA.Y - _epB.Y) * (_epC.X - _epB.X);
            return new Point2D(((_epC.Y - _epB.Y) * (temp1 - temp2) - (_epA.Y - _epB.Y) * (temp3 - temp2)) / temp4,
                ((_epA.X - _epB.X) * (temp3 - temp2) - (_epC.X - _epB.X) * (temp1 - temp2)) / temp4);
        }
        /// <summary>
        /// 垂心
        /// </summary>
        /// <returns>垂心</returns>
        public Point2D Orthocentre()
        {
            double a = _epB.Distance(_epC);
            double b = _epC.Distance(_epA);
            double c = _epA.Distance(_epB);
            double aDivCosA = (2 * a * b * c) / (Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(a, 2));// a/cosA
            double bDivCosB = (2 * a * b * c) / (Math.Pow(a, 2) + Math.Pow(c, 2) - Math.Pow(b, 2));
            double cDivCosC = (2 * a * b * c) / (Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2));
            return new Point2D((aDivCosA * _epA.X + bDivCosB * _epB.X + cDivCosC * _epC.X) / (aDivCosA + bDivCosB + cDivCosC),
                (aDivCosA * _epA.Y + bDivCosB * _epB.Y + cDivCosC * _epC.Y) / (aDivCosA + bDivCosB + cDivCosC));
        }
        /// <summary>
        /// 是否相似
        /// </summary>
        /// <param name="triangle">待比较三角形</param>
        /// <returns>相似返回true;否则false</returns>
        public bool IsSimilar(Triangle triangle)
        {
            //三边对应成比例则相似
            var thisEdges = GetEdgeLength();
            var otherEdges = triangle.GetEdgeLength();
            double temp = otherEdges[0] / thisEdges[0];
            return otherEdges[1].IsEqual(thisEdges[1] * temp) && otherEdges[2].IsEqual(thisEdges[2] * temp);
        }
        #endregion

        #region static
        /// <summary>
        /// 合法性检查
        /// </summary>
        /// <param name="vertexes">顶点</param>
        /// <returns>若能构成三角形则返回true;否则false</returns>
        public static bool ValidityCheck(params Point2D[] vertexes)
        {
            if (vertexes == null || vertexes.Length != 3)
                return false;
            if (MathUtilHelper.HaveRepeated(vertexes))
                return false;
            if (Parsifal.Math.Algorithm.GeometryAlgorithm.IsCollinearity(vertexes[0], vertexes[1], vertexes[2]))
                return false;
            return true;
        }
        #endregion
    }
}
