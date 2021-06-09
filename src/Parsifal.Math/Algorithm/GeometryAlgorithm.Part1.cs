using Parsifal.Math.Geometry;

namespace Parsifal.Math.Algorithm
{
    using Math = System.Math;
    public partial class GeometryAlgorithm
    {
        /// <summary>
        /// 是否共线
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <returns>共线时返回true;否则false</returns>
        public static bool IsCollinearity(in Point2D first, in Point2D second, in Point2D third)
        {
            //共线条件：(x2-x1)(y3-y1)=(x3-x1)(y2-y1)
            var temp1 = (second.X - first.X) * (third.Y - first.Y);
            var temp2 = (third.X - first.X) * (second.Y - first.Y);
            return temp1.IsEqual(temp2);
        }
        /// <summary>
        /// 是否为凸多边形
        /// </summary>
        /// <param name="vertexes">多边形顶点</param>
        /// <returns>如果是凸多边形则返回true;否则false</returns>
        public static bool IsConvexPolygon(in Point2D[] vertexes)
        {
            //任意两领边所组成的向量做叉乘，如果符号全部相同，则为凸多边形，有任一异号则为凹多边形
            int count = vertexes.Length;
            Point2D current, pre, next;
            int preCrossSign = 0;//上一叉积的符号，由于相邻三点不共线则不可能为0
            for (int i = 0; i < count; i++)
            {
                current = vertexes[i];
                if (i == 0)
                    pre = vertexes[count - 1];
                else
                    pre = vertexes[i - 1];
                if (i == count - 1)
                    next = vertexes[0];
                else
                    next = vertexes[i + 1];

                var v1 = current - pre;
                var v2 = next - current;
                var cross = Vector2D.CrossProduct(v1, v2);
                if (i == 0)
                    preCrossSign = System.Math.Sign(cross);
                else
                {
                    if (!cross.IsSameSign(preCrossSign))//分别与第一次做符号对比
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 多边形是否自相交
        /// </summary>
        /// <param name="vertexes">多边形顶点</param>
        /// <returns>如果自相交则返回true;否则false</returns>
        public static bool IsSelfIntersected(in Point2D[] vertexes)
        {
            int count = vertexes.Length;
            if (count <= 3)
                return false;
            int j, k, l, m;
            int comparaTime = (int)System.Math.Ceiling(count / 2d);//仅需比较半数的边
            for (int i = 0; i < comparaTime; i++)
            {
                //每条边分别与非邻边的边做相交判断，最大比较次数 n(n-3)/2
                j = (i == count - 1) ? 0 : i + 1;
                var currentSegment = (vertexes[i], vertexes[j]);
                k = (j == count - 1) ? 0 : j + 1;
                for (m = 0; m < count - 3; m++, k = (k == (count - 1)) ? 0 : k + 1)
                {
                    l = (k == count - 1) ? 0 : k + 1;
                    var compareSegment = (vertexes[k], vertexes[l]);
                    if (IsSegmentIntersection(currentSegment, compareSegment))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 线段是否相交
        /// </summary>
        /// <param name="checkSegment">待查线段</param>
        /// <param name="segment">线段</param>
        /// <returns>若<paramref name="checkSegment"/>与<paramref name="segment"/>相交则返回true;否则false</returns>
        public static bool IsSegmentIntersection(in Segment checkSegment, in Segment segment)
        {
            return IsSegmentIntersection((checkSegment.EpA, checkSegment.EpB), (segment.EpA, segment.EpB));
        }
        /// <summary>
        /// 线段是否相交
        /// </summary>
        /// <param name="checkSegment">待查线段</param>
        /// <param name="segment">线段</param>
        /// <returns>若<paramref name="checkSegment"/>与<paramref name="segment"/>相交则返回true;否则false</returns>
        public static bool IsSegmentIntersection(in (Point2D, Point2D) checkSegment, in (Point2D, Point2D) segment)
        {
            if (Math.Max(checkSegment.Item1.X, checkSegment.Item2.X) < Math.Min(segment.Item1.X, segment.Item2.X)
                || Math.Max(checkSegment.Item1.Y, checkSegment.Item2.Y) < Math.Min(segment.Item1.Y, segment.Item2.Y)
                || Math.Min(checkSegment.Item1.X, checkSegment.Item2.X) > Math.Max(segment.Item1.X, segment.Item2.X)
                || Math.Min(checkSegment.Item1.Y, checkSegment.Item2.Y) > Math.Max(segment.Item1.Y, segment.Item2.Y))
            {//两线段作为对角线的简单矩形不相交
                return false;
            }
            var c1 = Vector2D.CrossProduct(checkSegment.Item1 - checkSegment.Item2, segment.Item1 - checkSegment.Item2);
            var c2 = Vector2D.CrossProduct(checkSegment.Item1 - checkSegment.Item2, segment.Item2 - checkSegment.Item2);
            var c3 = Vector2D.CrossProduct(segment.Item1 - segment.Item2, checkSegment.Item1 - segment.Item2);
            var c4 = Vector2D.CrossProduct(segment.Item1 - segment.Item2, checkSegment.Item2 - segment.Item2);
            if (c1 * c2 > 0 || c3 * c4 > 0)
            {//如果大于零则表示线段两个端点都在另条线段的同一侧，此时不可能相交
                return false;
            }
            else
            {
                //如果c1=0,c2=0,则表示两线段共线(此时必有c3=0,c4=0),由于已排查外包矩形，此时两线段共线且交点为端点
                //小于0时则表示线段端点分别在另条线段两侧，同时满足则一定相交
                return true;
            }
        }
        /// <summary>
        /// 点是否在线段上
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="segment">线段</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="segment"/>上则返回true;否则false</returns>
        public static bool IsPointOnSegment(in Point2D checkPoint, in Segment segment)
        {
            return IsPointOnSegment(checkPoint, (segment.EpA, segment.EpB));
        }
        /// <summary>
        /// 点是否在线段上
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="segment">线段</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="segment"/>上则返回true;否则false</returns>
        public static bool IsPointOnSegment(in Point2D checkPoint, in (Point2D, Point2D) segment)
        {
            if (checkPoint == segment.Item1 || checkPoint == segment.Item2)
            {
                return true;
            }
            if (checkPoint.X > Math.Max(segment.Item1.X, segment.Item2.X)
                || checkPoint.X < Math.Min(segment.Item1.X, segment.Item2.X)
                || checkPoint.Y > Math.Max(segment.Item1.Y, segment.Item2.Y)
                || checkPoint.Y < Math.Min(segment.Item1.Y, segment.Item2.Y))
            {
                return false;
            }
            return Vector2D.CrossProduct(checkPoint - segment.Item2, segment.Item1 - segment.Item2).IsZero();
        }
        /// <summary>
        /// 点是否在三角形内
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="triangle">三角形</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="triangle"/>内时返回true;否则false</returns>
        public static bool IsPointInTranjgle(in Point2D checkPoint, in Triangle triangle)
        {
            var vertexes = triangle.GetVertexes();
            var pa = vertexes[0] - checkPoint;
            var pb = vertexes[1] - checkPoint;
            var pc = vertexes[2] - checkPoint;
            var t1 = Vector2D.CrossProduct(pa, pb);
            var t2 = Vector2D.CrossProduct(pb, pc);
            var t3 = Vector2D.CrossProduct(pc, pa);
            return t1.IsSameSign(t2) && t1.IsSameSign(t3);
        }
        /// <summary>
        /// 点是否在矩形内
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="rectangle">矩形</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="rectangle"/>内时返回true;否则false</returns>
        public static bool IsPointInRectangle(in Point2D checkPoint, in Rectangle rectangle)
        {
            var vertexes = rectangle.GetVertexes();
            //判断checkPoint是否同时包含于对边之间
            var c1 = Vector2D.CrossProduct(vertexes[1] - vertexes[0], checkPoint - vertexes[0]);
            var c2 = Vector2D.CrossProduct(vertexes[3] - vertexes[2], checkPoint - vertexes[2]);
            var c3 = Vector2D.CrossProduct(vertexes[2] - vertexes[1], checkPoint - vertexes[1]);
            var c4 = Vector2D.CrossProduct(vertexes[0] - vertexes[3], checkPoint - vertexes[3]);
            return (c1 * c2).IsGreaterOrEqual(0) && (c3 * c4).IsGreaterOrEqual(0);
        }
        /// <summary>
        /// 点是否在矩形内
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="rectangle">矩形(左,右,下,上)</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="rectangle"/>内时返回true;否则false</returns>
        public static bool IsPointInRectangle(in Point2D checkPoint, in (double, double, double, double) rectangle)
        {
            return checkPoint.X >= rectangle.Item1 && checkPoint.X <= rectangle.Item2
                && checkPoint.Y >= rectangle.Item3 && checkPoint.Y <= rectangle.Item4;
        }
        /// <summary>
        /// 点是否在多边形内
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="polygon">多边形边界</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="polygon"/>内时返回true;否则false</returns>
        public static bool IsPointInPolygon(in Point2D checkPoint, in Polygon polygon)
        {
            return IsPointInPolygon(checkPoint, polygon.GetVertexes());
        }
        /// <summary>
        /// 点是否在多边形内
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="polygon">多边形边界</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="polygon"/>内时返回true;否则false</returns>
        public static bool IsPointInPolygon(in Point2D checkPoint, in Point2D[] polygon)
        {
            if (!IsPointInRectangle(checkPoint, GetPolygonMBR(polygon)))
            {//点不在最小外包矩形内
                return false;
            }
            //从目标点出发引一条任意方向的射线，看这条射线和多边形所有边的交点数目。如果有奇数个交点则说明在内部，如果有偶数个交点则在外部
            int count = 0;
            Point2D pA, pB;
            pA = polygon[0];
            for (int i = 1; i <= polygon.Length; i++)
            {
                pB = polygon[i % polygon.Length];
                if (IsPointOnSegment(checkPoint, (pA, pB)))//在边上时直接返回
                    return true;
                double slope = (pB.Y - pA.Y) / (pB.X - pA.X);
                bool cond1 = (pA.X <= checkPoint.X) && (checkPoint.X < pB.X);
                bool cond2 = (pB.X <= checkPoint.X) && (checkPoint.X < pA.X);
                bool above = (checkPoint.Y < slope * (checkPoint.X - pA.X) + pA.Y);
                if ((cond1 || cond2) && above)
                    count++;
                pA = pB;
            }

            return count % 2 != 0;
        }
        /// <summary>
        /// 点是否在圆内
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="circle">圆</param>
        /// <returns>若<paramref name="checkPoint"/>在<paramref name="circle"/>内返回true;否则false</returns>
        public static bool IsPointInCircle(in Point2D checkPoint, in Circle circle)
        {
            return checkPoint.Distance(circle.Center) <= circle.Radius;
        }
        /// <summary>
        /// 线段是否在多边形内
        /// </summary>
        /// <param name="checkSegment">待查线段</param>
        /// <param name="polygon">多边形</param>
        /// <returns>若<paramref name="checkSegment"/>在<paramref name="polygon"/>内则返回true;否则false</returns>
        public static bool IsSegmentInPolygon(in Segment checkSegment, in Polygon polygon)
        {
            return IsSegmentInPolygon(checkSegment, polygon.GetVertexes());
        }
        /// <summary>
        /// 线段是否在多边形内
        /// </summary>
        /// <param name="checkSegment">待查线段</param>
        /// <param name="polygon">多边形</param>
        /// <returns>若<paramref name="checkSegment"/>在<paramref name="polygon"/>内则返回true;否则false</returns>
        public static bool IsSegmentInPolygon(in Segment checkSegment, in Point2D[] polygon)
        {
            return IsSegmentInPolygon((checkSegment.EpA, checkSegment.EpB), polygon);
        }
        /// <summary>
        /// 线段是否在多边形内
        /// </summary>
        /// <param name="checkSegment">待查线段</param>
        /// <param name="polygon">多边形</param>
        /// <returns>若<paramref name="checkSegment"/>在<paramref name="polygon"/>内则返回true;否则false</returns>
        public static bool IsSegmentInPolygon(in (Point2D, Point2D) checkSegment, in Point2D[] polygon)
        {
            if (!IsPointInPolygon(checkSegment.Item1, polygon) || !IsPointInPolygon(checkSegment.Item2, polygon))
            {//两端点有任一点不在多边形内
                return false;
            }
            if (IsConvexPolygon(polygon))
            {//凸多边形
             //凸多边形时如线段端点在内则线段本身一定在内
                return true;
            }
            else
            {//凹多边形
             //如果线段和多边形的某条边内交(两线段内交是指两线段相交且交点不在两线段的端点)，则线段肯定不在多边形内
                var pointSet = new System.Collections.Generic.List<Point2D>();
                Point2D pA, pB;
                pA = polygon[0];
                for (int i = 1; i < polygon.Length; i++)
                {
                    pB = polygon[i % polygon.Length];
                    if (IsPointOnSegment(checkSegment.Item1, (pA, pB)))//如果线段端点在某边上
                        pointSet.Add(checkSegment.Item1);
                    else if (IsPointOnSegment(checkSegment.Item2, (pA, pB)))
                        pointSet.Add(checkSegment.Item2);
                    else if (IsPointOnSegment(pA, checkSegment))//如果边的端点在线段上
                        pointSet.Add(pA);
                    else if (IsPointOnSegment(pB, checkSegment))
                        pointSet.Add(pB);
                    else if (IsSegmentIntersection((pA, pB), checkSegment))//该线段与边相交
                    {//此时两线段一定内交
                        return false;
                    }
                    pA = pB;
                }
                //如果多边形的某个顶点和线段相交，则必须判断两相交交点之间的线段是否包含于多边形内
                pointSet.Sort(Point2DComparison);
                if (pointSet.Count > 0)
                {
                    pA = pointSet[0];
                    for (int i = 1; i < pointSet.Count; i++)
                    {
                        pB = pointSet[i % pointSet.Count];
                        var midpoint = new Point2D((pA.X + pB.X) / 2, (pA.Y + pB.Y) / 2);
                        if (!IsPointInPolygon(midpoint, polygon))
                        {//相邻两点组成线段的中点不在多边形内
                            return false;
                        }
                        pA = pB;
                    }
                }
                return true;
            }

            static int Point2DComparison(Point2D pointA, Point2D pointB)
            {//X坐标小的排在前，X相同时Y坐标小的排在前
                if (pointA.X < pointB.X)
                    return -1;
                else if (pointA.X == pointB.X)
                {
                    if (pointA.Y < pointB.Y)
                        return -1;
                    else if (pointA.Y == pointB.Y)
                        return 0;
                    else
                        return 1;
                }
                else
                    return 1;
            }
        }
        /// <summary>
        /// 多边形是否在多边形内
        /// </summary>
        /// <param name="checkPolygon">待查多边形</param>
        /// <param name="polygon">多边形</param>
        /// <returns>若<paramref name="checkPolygon"/>在<paramref name="polygon"/>内则返回true;否则false</returns>
        public static bool IsPolygonInPolygon(in Polygon checkPolygon, in Polygon polygon)
        {
            return IsPolygonInPolygon(checkPolygon.GetVertexes(), polygon.GetVertexes());
        }
        /// <summary>
        /// 多边形是否在多边形内
        /// </summary>
        /// <param name="checkPolygon">待查多边形</param>
        /// <param name="polygon">多边形</param>
        /// <returns>若<paramref name="checkPolygon"/>在<paramref name="polygon"/>内则返回true;否则false</returns>
        public static bool IsPolygonInPolygon(in Point2D[] checkPolygon, in Polygon polygon)
        {
            return IsPolygonInPolygon(checkPolygon, polygon.GetVertexes());
        }
        /// <summary>
        /// 多边形是否在多边形内
        /// </summary>
        /// <param name="checkPolygon">待查多边形</param>
        /// <param name="polygon">多边形</param>
        /// <returns>若<paramref name="checkPolygon"/>在<paramref name="polygon"/>内则返回true;否则false</returns>
        public static bool IsPolygonInPolygon(in Polygon checkPolygon, in Point2D[] polygon)
        {
            return IsPolygonInPolygon(checkPolygon.GetVertexes(), polygon);
        }
        /// <summary>
        /// 多边形是否在多边形内
        /// </summary>
        /// <param name="checkPolygon">待查多边形</param>
        /// <param name="polygon">多边形</param>
        /// <returns>若<paramref name="checkPolygon"/>在<paramref name="polygon"/>内则返回true;否则false</returns>
        public static bool IsPolygonInPolygon(in Point2D[] checkPolygon, in Point2D[] polygon)
        {
            Point2D pA, pB;
            pA = checkPolygon[0];
            for (int i = 1; i < checkPolygon.Length; i++)
            {
                pB = checkPolygon[i % checkPolygon.Length];
                //每条边都需要在多边形内,有任一边不在则直接返回
                if (!IsSegmentInPolygon((pA, pB), polygon))
                {
                    return false;
                }
                pA = pB;
            }
            return true;
        }
    }
}
