namespace Parsifal.Math.Geometry
{
    /// <summary>
    /// 封闭图形
    /// </summary>
    public interface ICloseShape
    {
        /// <summary>
        /// 计算面积
        /// </summary>
        /// <returns>面积</returns>
        double GetArea();
        /// <summary>
        /// 计算周长
        /// </summary>
        /// <returns>周长</returns>
        double GetPerimeter();
    }
}
