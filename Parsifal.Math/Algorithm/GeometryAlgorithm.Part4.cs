using System.Linq;
using Parsifal.Math.Geometry;

namespace Parsifal.Math.Algorithm
{
    public partial class GeometryAlgorithm
    {
        /// <summary>
        /// 获取多边形的最小外包矩形
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <returns>外包矩形(最小X,最大X,最小Y,最大Y)</returns>
        public static (double, double, double, double) GetPolygonMBR(Polygon polygon)
        {
            return GetPolygonMBR(polygon.GetVertexes());
        }
        /// <summary>
        /// 获取多边形的最小外包矩形
        /// </summary>
        /// <param name="polygon">多边形顶点</param>
        /// <returns>外包矩形(最小X,最大X,最小Y,最大Y)</returns>
        public static (double, double, double, double) GetPolygonMBR(Point2D[] polygon)
        {
            double minX = polygon.Min(p => p.X);
            double maxX = polygon.Max(p => p.X);
            double minY = polygon.Min(p => p.Y);
            double maxY = polygon.Max(p => p.Y);
            return (minX, maxX, minY, maxY);
        }
        /// <summary>
        /// 获取圆的最小外包矩形
        /// </summary>
        /// <param name="circle"></param>
        /// <returns>外包矩形(最小X,最大X,最小Y,最大Y)</returns>
        public static (double, double, double, double) GetCircleMBR(Circle circle)
        {
            return (circle.Center.X - circle.Radius, circle.Center.X + circle.Radius,
                circle.Center.Y - circle.Radius, circle.Center.Y + circle.Radius);
        }
    }
}
