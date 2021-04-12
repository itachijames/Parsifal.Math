namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// 简单多边形
    /// </summary>
    public interface IPolygon : ICloseShape
    {
        /// <summary>
        /// 边数
        /// </summary>
        int EdgeNumber { get; }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <returns>顶点集合</returns>
        Point2D[] GetVertexes();
        /// <summary>
        /// 是否为凸多边形
        /// </summary>
        /// <returns>凸多边形返回true;凹多边形则返回false</returns>
        bool IsConvex();
    }
}
