using Parsifal.Math.Geometry;

namespace Parsifal.Math.Algorithm
{
    /// <summary>
    /// 几何算法
    /// </summary>
    /// <remarks>基于欧几里得几何(Euclid geometry),提供空间几何关系相关功能。
    /// 注意：为保证性能,类中方法均假定传入参数合法,不进行参数合法性检测
    /// </remarks>
    public partial class GeometryAlgorithm
    {
        /// <summary>
        /// 点到线段的距离
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="segment">线段</param>
        /// <param name="nearest">最近点</param>
        /// <returns><paramref name="checkPoint"/>到<paramref name="segment"/>的距离</returns>
        public static double Point2Segment(in Point2D checkPoint, in Segment segment, out Point2D nearest)
        {
            return Point2Segment(checkPoint, (segment.EpA, segment.EpB), out nearest);
        }
        /// <summary>
        /// 点到线段的距离
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="segment">线段</param>
        /// <param name="nearest">最近点</param>
        /// <returns><paramref name="checkPoint"/>到<paramref name="segment"/>的距离</returns>
        public static double Point2Segment(in Point2D checkPoint, in (Point2D, Point2D) segment, out Point2D nearest)
        {
            //先判断点是否在线端外靠近某一端点，若点投影在线段内则再求垂足点
            var ab = segment.Item2 - segment.Item1;
            var ac = checkPoint - segment.Item1;
            double f = Vector2D.DotProduct(ab, ac);
            if (f <= 0)
            {
                return checkPoint.Distance(nearest = segment.Item1);
            }
            double d = Vector2D.DotProduct(ab, ab);
            if (f >= d)
            {
                return checkPoint.Distance(nearest = segment.Item2);
            }
            double r = f / d;
            //垂足点
            nearest = new Point2D(segment.Item1.X + (segment.Item2.X - segment.Item1.X) * r, segment.Item1.Y + (segment.Item2.Y - segment.Item1.Y) * r);
            return checkPoint.Distance(nearest);
        }
        /// <summary>
        /// 点到多边形距离
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="polygon">多边形</param>
        /// <param name="nearest">最近点</param>
        /// <returns><paramref name="checkPoint"/>到<paramref name="polygon"/>的距离</returns>
        public static double Point2Polygon(in Point2D checkPoint, in Polygon polygon, out Point2D nearest)
        {
            return Point2Polygon(checkPoint, polygon.GetVertexes(), out nearest);
        }
        /// <summary>
        /// 点到多边形距离
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="polygon">多边形</param>
        /// <param name="nearest">最近点</param>
        /// <returns><paramref name="checkPoint"/>到<paramref name="polygon"/>的距离</returns>
        public static double Point2Polygon(in Point2D checkPoint, in Point2D[] polygon, out Point2D nearest)
        {
            //求出点到各边的距离，然后取最小值
            nearest = Point2D.Origin;
            double minDistance = 0d;
            Point2D pA, pB;
            pA = polygon[0];
            for (int i = 1; i <= polygon.Length; i++)
            {
                pB = polygon[i % polygon.Length];
                double distance = Point2Segment(checkPoint, (pA, pB), out var temp);
                if (i == 1 || distance < minDistance)
                {//求最小
                    minDistance = distance;
                    nearest = temp;
                }
                pA = pB;
            }
            return minDistance;
        }
        /// <summary>
        /// 点到圆的距离
        /// </summary>
        /// <param name="checkPoint">待查点</param>
        /// <param name="circle">圆</param>
        /// <param name="nearest">最近点</param>
        /// <returns><paramref name="checkPoint"/>到<paramref name="circle"/>的距离</returns>
        public static double Point2Circle(in Point2D checkPoint, in Circle circle, out Point2D nearest)
        {
            if (checkPoint == circle.Center)
            {
                //todo 是否取圆上任意点
                nearest = Point2D.NotPoint;
                return circle.Radius;
            }
            var cn = (checkPoint - circle.Center).Normalize() * circle.Radius;
            nearest = circle.Center + cn;
            return checkPoint.Distance(nearest);
        }
    }
}
